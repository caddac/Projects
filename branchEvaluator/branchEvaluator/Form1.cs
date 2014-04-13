using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace branchEvaluator
{
    public partial class Form1 : Form
    {
        public int programLen;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBranch_Click(object sender, EventArgs e)
        {
            String[] lines = txtbxInput.Lines;//all input on single line, put into lines[0]
            char[] clines = lines[0].ToArray();//make into char array
            programLen = clines.Length;
           Program.runIt(clines);//runit, duh!
        }

        public void print(string printString)
        {
            txtbxOutput.AppendText(printString);
        }

        public void printTree(token root, int indent)
        {
            if (root.getPrecedence() == -1)//pointing at terminator
                return;
            printTree(root.getRight(), indent + 1);
            for (int i = 0; i < indent; i++)
                txtbxOutput.AppendText("   ");
            txtbxOutput.AppendText(root.getValue() + " lowest leaf: " + root.getOperandOrder() + " TD: " + root.getTD() + " FD: " + root.getFD() + "\n");
            printTree(root.getLeft(), indent + 1);
        }

        public void evalTree(token root)
        {
            if (!(root.getValue() == "!"))
            {
                if (root.getLeft().getValue() == "$")
                {
                    return;
                }
                evalTree(root.getLeft());
                root.setOperandOrder(root.getLeft().getOperandOrder());
            }
            

            evalTree(root.getRight());
            if (root.getValue() == "!")
                root.setOperandOrder(root.getRight().getOperandOrder());
            
        }

        public void createBranch(token root)
        {
          /*  if (root.getLeft().getValue() == "$")
            {
                return;
            }*/

            if (root.getParent() == null)
            {
                root.setFD(token.getNumberOfOperands() + 1);
                root.setTD(token.getNumberOfOperands());
            }
            else //if (!root.isOperand())//if operator
            {
                switch (root.getParent().getValue())
                {
                    case "&":
                        if (root.getIsRightChild())
                            root.setTD(root.getParent().getTD());
                        else
                            root.setTD(root.getParent().getRight().getLowestLeftLeaf());
                        root.setFD(root.getParent().getFD());
                        break;
                    case "|":
                        root.setTD(root.getParent().getTD());
                        if (root.getIsRightChild())
                            root.setFD(root.getParent().getFD());
                        else
                            root.setFD(root.getParent().getRight().getLowestLeftLeaf());
                        break;
                    case "!":
                        root.setTD(root.getParent().getFD());
                        root.setFD(root.getParent().getTD());
                        break;
                }
            }
            if (root.getValue() == "$")
                return;

            createBranch(root.getLeft());


            createBranch(root.getRight());

        }

        public void printBranching(token root)
        {
            if (root.getLeft().getValue() == "$" && !(root.getValue() == "!"))
            {
                //oporder, brt or brf, location, value
                string brtVal;
                if (root.getTD() == root.getOperandOrder() + 1)
                    brtVal = "brF " + root.getFD().ToString();//
                else
                    brtVal = "brT " + root.getTD().ToString();
                txtbxOutput.AppendText(root.getOperandOrder() + "  " + brtVal + "  " + root.getValue() + "\n");
                return;
            }
            if (!(root.getValue() == "!"))
                printBranching(root.getLeft());

            printBranching(root.getRight());
        }





        /*
         * at every node, starting with the most left one, 
         * 
         * if my parent is a '!'
         * 
         * if my parent is a '&'
         * @ left child-> store left and check @ right child 
         * @ right child ->if left == right == T--> '&' node evals to T:else eval to false
         * if my parent is '|'
         * @ left child -> if T->parent '|' node evals true
         * @ right child ->
         * 
         * 
         * 
         * |: either child is true, I'm true
         *      both children are false, i'm  false
         *      
         * &: both children are true, I'm true
         *      either child is false, I'm false
         *      
         * !: right child is true, I'm false
         *      right child is false, I'm true
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * */


    }
}
