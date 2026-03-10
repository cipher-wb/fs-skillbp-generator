using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NodeEditor
{
    public class NodeAutoLayouter
    {
        [System.Flags]
        public enum CalculateMode
        {
            // 水平计算模式
            Horizontal = 1,

            // 垂直计算模式
            Vertical = Horizontal << 1,

            // 从上到下，从左到右
            Positive = Horizontal << 2,

            // 从下到上，从右到左
            Negative = Horizontal << 3
        }

        public class TreeNode
        {
            public float w, h;
            public float x, y, prelim, mod, shift, change;
            public TreeNode tl, tr;              
            public TreeNode el, er;
            public float msel, mser;
            public List<TreeNode> children = new List<TreeNode>();
            public int ChildrenCount => children.Count;
            public CalculateMode CalculateMode;

            public TreeNode(float w, float h, float y,
                CalculateMode calculateMode = CalculateMode.Horizontal | CalculateMode.Positive)
            {
                this.w = w;
                this.h = h;
                this.y = y;
                this.CalculateMode = calculateMode;
            }

            public void AddChild(TreeNode child)
            {
                this.children.Add(child);
            }

            public Vector2 GetPos()
            {
                var calculateResult = new Vector2(x, y);

                // 根节点位于原点，绕原点进行旋转(绕原点的旋转矩阵)，并对节点位置进行修正（因为宽高翻转）
                if (CalculateMode == (NodeAutoLayouter.CalculateMode.Horizontal | NodeAutoLayouter.CalculateMode.Negative))
                {
                    Vector2 temp = calculateResult;
                    temp.x = -calculateResult.y - h;
                    temp.y = calculateResult.x;
                    calculateResult = temp;
                }

                if (CalculateMode == (NodeAutoLayouter.CalculateMode.Horizontal | NodeAutoLayouter.CalculateMode.Positive))
                {
                    Vector2 temp = calculateResult;
                    temp.x = calculateResult.y;
                    temp.y = calculateResult.x;
                    calculateResult = temp;
                }

                if (CalculateMode == (NodeAutoLayouter.CalculateMode.Vertical | NodeAutoLayouter.CalculateMode.Negative))
                {
                    calculateResult.y = -calculateResult.y - h;
                }

                return calculateResult;
            }
        }

        public static void Layout(INodeForLayoutConvertor nodeForLayoutConvertor)
        {
            if (nodeForLayoutConvertor.PrimNode2LayoutNode() == null)
            {
                return;
            }

            FirstWalk(nodeForLayoutConvertor.LayoutRootNode);
            SecondWalk(nodeForLayoutConvertor.LayoutRootNode, 0);

            nodeForLayoutConvertor.LayoutNode2PrimNode();
        }

        static void FirstWalk(TreeNode t)
        {
            if (t.ChildrenCount == 0)
            {
                SetExtremes(t);
                return;
            }

            FirstWalk(t.children[0]);
            IYL ih = UpdateIYL(Bottom(t.children[0].el), 0, null);
            for (int i = 1; i < t.ChildrenCount; i++)
            {
                FirstWalk(t.children[i]);
                float minY = Bottom(t.children[i].er);
                Seperate(t, i, ih);
                ih = UpdateIYL(minY, i, ih);
            }

            PositionRoot(t);
            SetExtremes(t);
        }

        static void SetExtremes(TreeNode t)
        {
            if (t.ChildrenCount == 0)
            {
                t.el = t;
                t.er = t;
                t.msel = t.mser = 0;
            }
            else
            {
                t.el = t.children[0].el;
                t.msel = t.children[0].msel;
                t.er = t.children[t.ChildrenCount - 1].er;
                t.mser = t.children[t.ChildrenCount - 1].mser;
            }
        }

        static void Seperate(TreeNode t, int i, IYL ih)
        {
            TreeNode sr = t.children[i - 1];
            float mssr = sr.mod;
            TreeNode cl = t.children[i];
            float mscl = cl.mod;
            while (sr != null && cl != null)
            {
                if (Bottom(sr) > ih.lowY) ih = ih.nxt;

                float dist = (mssr + sr.prelim + sr.w) - (mscl + cl.prelim);
                if (dist > 0)
                {
                    mscl += dist;
                    MoveSubtree(t, i, ih.index, dist);
                }

                float sy = Bottom(sr), cy = Bottom(cl);
                if (sy <= cy)
                {
                    sr = NextRightContour(sr);
                    if (sr != null) mssr += sr.mod;
                }

                if (sy >= cy)
                {
                    cl = NextLeftContour(cl);
                    if (cl != null) mscl += cl.mod;
                }
            }

            if (sr == null && cl != null) SetLeftThread(t, i, cl, mscl);
            else if (sr != null && cl == null) SetRightThread(t, i, sr, mssr);
        }

        static void MoveSubtree(TreeNode t, int i, int si, float dist)
        {
            t.children[i].mod += dist;
            t.children[i].msel += dist;
            t.children[i].mser += dist;
            DistributeExtra(t, i, si, dist);
        }

        static TreeNode NextLeftContour(TreeNode t) { return t.ChildrenCount == 0 ? t.tl : t.children[0]; }
        static TreeNode NextRightContour(TreeNode t) { return t.ChildrenCount == 0 ? t.tr : t.children[t.ChildrenCount - 1]; }
        static float Bottom(TreeNode t) { return t.y + t.h; }

        static void SetLeftThread(TreeNode t, int i, TreeNode cl, float modsumcl)
        {
            TreeNode li = t.children[0].el;
            li.tl = cl;
            float diff = (modsumcl - cl.mod) - t.children[0].msel;
            li.mod += diff;
            li.prelim -= diff;
            t.children[0].el = t.children[i].el;
            t.children[0].msel = t.children[i].msel;
        }

        static void SetRightThread(TreeNode t, int i, TreeNode sr, float modsumsr)
        {
            TreeNode ri = t.children[i].er;
            ri.tr = sr;
            float diff = (modsumsr - sr.mod) - t.children[i].mser;
            ri.mod += diff;
            ri.prelim -= diff;
            t.children[i].er = t.children[i - 1].er;
            t.children[i].mser = t.children[i - 1].mser;
        }

        static void PositionRoot(TreeNode t)
        {
            t.prelim = (t.children[0].prelim + t.children[0].mod + t.children[t.ChildrenCount - 1].mod +
                        t.children[t.ChildrenCount - 1].prelim + t.children[t.ChildrenCount - 1].w) / 2 - t.w / 2;
        }

        static void SecondWalk(TreeNode t, float modsum)
        {
            modsum += t.mod;
            t.x = t.prelim + modsum;
            AddChildSpacing(t);
            for (int i = 0; i < t.ChildrenCount; i++) SecondWalk(t.children[i], modsum);
        }

        static void DistributeExtra(TreeNode t, int i, int si, float dist)
        {
            if (si != i - 1)
            {
                float nr = i - si;
                t.children[si + 1].shift += dist / nr;
                t.children[i].shift -= dist / nr;
                t.children[i].change -= dist - dist / nr;
            }
        }


        static void AddChildSpacing(TreeNode t)
        {
            float d = 0, modsumdelta = 0;
            for (int i = 0; i < t.ChildrenCount; i++)
            {
                d += t.children[i].shift;
                modsumdelta += d + t.children[i].change;
                t.children[i].mod += modsumdelta;
            }
        }

        class IYL
        {
            public float lowY;
            public int index;
            public IYL nxt;

            public IYL(float lowY, int index, IYL nxt)
            {
                this.lowY = lowY;
                this.index = index;
                this.nxt = nxt;
            }
        }

        static IYL UpdateIYL(float minY, int i, IYL ih)
        {
            while (ih != null && minY >= ih.lowY) ih = ih.nxt;
            return new IYL(minY, i, ih);
        }
    }
}
