using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace branchEvaluator
{
    static class Program
    {
        public static Form1 myform;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            myform = new Form1();
            Application.Run(myform);
        }
        public static token terminator = new token("$");

        public static void runIt(char[] postfixArray)
        {
            Stack<token> postStack = new Stack<token>();//postfix stack
            Stack<token> evalstack = new Stack<token>();//stack where tree is built
            List<token> postList = new List<token>();


            //postStack.Push(new token("$"));
            foreach (char s in postfixArray)//make new tokens and push to postfix stack
            {
                //postStack.Push(new token(s.ToString()));//push onto stack
                postList.Add(new token(s.ToString()));//push into list
            }
            //postStack.Push(new token("#"));

            foreach (token tok in postList)
            {
                //MessageBox.Show(tok.getValue());
                if (tok.isOperand())
                {
                    tok.setLeft(terminator);
                    tok.setRight(terminator);
                    evalstack.Push(tok);//store on eval stack
                }
                else if (tok.getValue() == "!")
                {
                    tok.setRight(evalstack.Peek());
                    evalstack.Peek().setToRightChild();
                    evalstack.Pop().setParent(tok);
                    tok.setLeft(terminator);
                    evalstack.Push(tok);
                }
                else
                {
                    tok.setRight(evalstack.Peek());
                    evalstack.Peek().setToRightChild();
                    evalstack.Pop().setParent(tok);
                    tok.setLeft(evalstack.Peek());
                    evalstack.Peek().setToLeftChild();
                    evalstack.Pop().setParent(tok);
                    evalstack.Push(tok);
                }
            }
            string postFixString = string.Empty;
            foreach (char s in postfixArray)
                postFixString += s;

            myform.print(postFixString + "\n");
            myform.evalTree(evalstack.Peek());
            myform.createBranch(evalstack.Peek());
            myform.printTree(evalstack.Peek(), 0);
            myform.printBranching(evalstack.Peek());
            
            token root = evalstack.Peek();
            int start = 0;
            bool die = false;
            //token.getToken(0, root);

            token currentNode ;
            //int progCounter = 0;

            while (start < token.getNumberOfOperands())
            {
                token.getToken(start, root);
                currentNode = token.getTheOne();
                
                myform.print("At state " + currentNode.getOperandOrder() + "\n");

                if (currentNode.getValue() == "T")
                {
                    start = currentNode.getTD();
                }
                else if (currentNode.getValue() == "F")
                {
                    start = currentNode.getFD();
                }
                else if (currentNode.getValue() == "D")
                {
                    myform.print("Die");
                    die = true;
                    break;
                }
            }

            if (!die)
                if (start == token.getNumberOfOperands())
                    myform.print("Evaluates to True");
                else
                    myform.print("Evaluates to False");



        }
    }
}
