using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace branchEvaluator
{
    public class token
    {
        private static int opCounter = 0;
        public static int getNumberOfOperands() { return opCounter; }

        private static int postCounter = 0;
        private static Dictionary<string, int> precedenceDict = new Dictionary<string, int>
        {
            {"T",0},
            {"|",1},
            {"&",2},
            {"!",3},
            {"F",0},
            {"$",-1}
        };

        private static token theOne;
        public static token getTheOne() { return theOne; }

        public static void getToken(int opOrder, token root)
        {
            if (root.getLeft().getValue() == "$" && !(root.getValue() == "!"))
            {
                if (opOrder == root.getOperandOrder())
                {
                    theOne = root;
                    return;
                }
                else
                    return;
            }
            if (!(root.getValue() == "!"))
                getToken(opOrder, root.getLeft());
            getToken(opOrder, root.getRight());
            return;
        }


        //values
        private string value;
        private int precedence = -1;
        private int postFixOrder = -1;
        private Boolean operand = false;
        private token left;
        private token right;
        private token parent;
        private int opOrder = -1;
        private int TD;
        private int FD;
        private bool isRightChild;

        //gets
        public token getLeft() { return left; }
        public token getRight() { return right; }
        public token getParent() { return parent; }
        public bool isOperand() { return operand; }
        public int getPrecedence() { return precedence; }
        public string getValue() { return value; }
        public int getTD() { return TD; }
        public int getFD() { return FD; }
        public int getOperandOrder() { return opOrder; }
        public int getLowestLeftLeaf() { return opOrder; }
        public bool getIsRightChild() { return isRightChild; }

        //public int getLowestLeaf() { return lowestLeaf; }
        //sets
        public void setLeft(token input) {  left = input; }
        public void setRight(token input) { right = input; }
        public void setParent(token input) { parent = input; }
        public void setFD(int input) { FD = input; }
        public void setTD(int input) { TD = input; }
        public void setOperandOrder(int input) { opOrder = input; }
        public void setToRightChild() { isRightChild = true; }
        public void setToLeftChild() { isRightChild = false; }


        public token(string token)
        {
            value = token;
            postFixOrder = ++postCounter;
            precedenceDict.TryGetValue(value, out precedence);
            if (precedence == 0)
            {
                opOrder = opCounter++;
                operand = true;
            }

        }
       /* public token getTokenByPostOrder(int index)
        {

        }*/
    }
}
