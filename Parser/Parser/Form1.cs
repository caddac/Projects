using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
/*
 * This is Dr. Weems code modified to an object oriented approach...sort of
 * 
 * 
 * 
 * */
namespace Parser
{

    public struct astType
    {
        public int left, right;
    } ;

    public partial class Form1 : Form
    {
        Token[] program = new Token[1000];
        int programSize;
        
        int[] operatorStack = new int[2000]; // entries point to program[]
        int operatorSP = -1;     // initial empty stack

        int nextSymbol; // Simple scan

        int[] postfixIndex = new int[1000]; // Needed to build AST
        int postfixLength = 0;
        int waitingOperands = 0; // Indicates hypothetical unconsumed operands
        int colonCount = 0, questionCount = 0;


        astType[] ast = new astType[1000];

        /// <summary>
        /// ///////////////////////////////////////////////////////
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            txtbxOutput.Clear();
            try
            {
                Token.removeOperands();

            }
            catch{}
            runIt();
        }

        
        /// <summary>
       /// reads all lines from the text box, creates new token objects 
       /// </summary>
 
        private void readInput()
        {
            int i,j,n,m,p;
            string[] temp = new string[500];
            temp = txtbxInput.Lines;
            string[] nums = temp[0].Split(' ');
            colonCount = questionCount = 0;



            m = int.Parse( nums[0]);
            n = int.Parse(nums[1]);
            p = int.Parse(nums[2]);

            for (i = 1; i < m+1; i++)//going through m
            {
               // if (Token.isName(temp[i]))
                    Token.addOperand(temp[i], "bool");//add to opdict
                //else
                    //MessageBox.Show("Error in input\nLine " + i.ToString());
            }
            for (i = m+1; i < n+m+1; i++)//going thorugh n
            {
               // if (Token.isName(temp[i]))
                    Token.addOperand(temp[i], "int");//add to opdict
                //else
                    //MessageBox.Show("Error in input\nLine " + i.ToString());
            }
            program[0] = new Token("$", "terminator");
            for (i = n + m + 1; i < p + n + m + 1; i++)//going through p
            {
                string[] split = temp[i].Split(' ');

                if (split[0] == "?") questionCount++ ;
                if (split[0] == ":") colonCount++ ;

                program[i -( n + m )] = new Token(split[0], split[1]);
            }

            program[p + 1] = new Token("#", "terminator");
            programSize=p+2;
            if (colonCount != questionCount)
                MessageBox.Show("Unmatched ? or : in Input");
 //check for $ and # removed
        }
        
  

        private void checkAdjacentSymbols(Token first, Token second)//verifies that two adjacent characters have an operator among them
        { // Streamlined check on adjacent symbols in input
            bool firstIsOperand = first.getToken() == ")" || first.isIdentifier() || first.getOpType() == "pic",//Token.isName(first),//set true if first is letter or ")"
                secondIsOperand = second.isIdentifier() || second.getToken() == "(" || second.getToken() == "!" || second.getOpType() == "pic";  //set true if second is a letter or "(" or a "!"

            if (firstIsOperand == secondIsOperand)
            {
                txtbxOutput.AppendText(first + " followed by " + second);
                MessageBox.Show("stop"); Application.Exit();
            }
        }

        void translate()//
        {
            string lastSymbol;  // For detecting improper adjacent symbols

            lastSymbol = "(";   // Safe way to initialize this
            operatorStack[++operatorSP] = 0;  // push initial $ to stack
            nextSymbol = 0;
            for (nextSymbol = 1; nextSymbol < programSize; nextSymbol++)//counts through program string
            {
                checkAdjacentSymbols(program[nextSymbol - 1], program[nextSymbol]);//verifies every two characters contain 1 operator
                if (program[nextSymbol].isIdentifier())//if nextsymbol is an identifier
                {
                    postfixIndex[postfixLength++] = nextSymbol;//push onto postfixIndex(stack)
                    waitingOperands++;							//add to count
                }
                else if (program[nextSymbol].getToken() == "(" || program[nextSymbol].getToken() == "!" )//if it's not an identifer, if "(" or "!" 
                {

                    operatorStack[++operatorSP] = nextSymbol;//push onto operator stack!
                }
                else
                {
                    // Move ripe operators to postfix.  Everything is left-associative
                    while ((program[nextSymbol].getPrecedence()) < (program[operatorStack[operatorSP]].getPrecedence()))    //while nextsymbols precedence is <= top operator on the stacks precedence
                    {
                        switch (program[operatorStack[operatorSP]].getToken())//pull operator on top of stack
                        {
                            case "(":
                                MessageBox.Show("Parenthesis mismatch detected at pos " + nextSymbol + "\nFirst Case statement"); //Application.Exit();
                                break;
                            case "!":
                                if (waitingOperands < 1)
                                {
                                    MessageBox.Show("No operands for ! at position " + operatorStack[operatorSP] + "\nSecond Case Statement"); Application.Exit();
                                }
                                postfixIndex[postfixLength++] = operatorStack[operatorSP];
                                break;
                            case "*":
                            case "/":
                            case "+":
                            case "-":
                            case "<":
                            case ">":
                            case "<=":
                            case ">=":
                            case "&&":
                            case "||":
                            case "==":
                            case "?":
                            case ":":
                                if (waitingOperands < 2)
                                {
                                    MessageBox.Show("Only " + waitingOperands + " operands for " + program[operatorStack[operatorSP]] + " at position " + operatorStack[operatorSP]);
                                    Application.Exit();
                                }
                                postfixIndex[postfixLength++] = operatorStack[operatorSP];
                                waitingOperands--;
                                break;
                            default:
                                txtbxOutput.AppendText("Uncovered case: " + program[operatorStack[operatorSP]]);
                                break;
                        } // end switch

                        operatorSP--;
                    } // end while
                    
                    if (program[nextSymbol].getToken() == ":")
                    {
                        if (program[nextSymbol].getToken() == ":" && program[operatorStack[operatorSP]].getToken() == ":")
                        {
                            postfixIndex[postfixLength++] = operatorStack[operatorSP];
                            waitingOperands--; operatorSP--;
                            postfixIndex[postfixLength++] = operatorStack[operatorSP];
                            waitingOperands--; operatorSP--;
                        }
                        if (program[nextSymbol].getToken() == ":" && program[operatorStack[operatorSP]].getToken() != "?")
                        {
                            MessageBox.Show("Unmatched : with ?");
                        }

                        operatorStack[++operatorSP] = nextSymbol;//push : on the stack
                    }


                    else if (program[nextSymbol].getToken() == ")")//checks to see if end of expression
                        if (program[operatorStack[operatorSP]].getToken() == "(")//check for beginning of expression on the stack
                            operatorSP--;//pop if it is there
                        else
                        {
                            txtbxOutput.AppendText(") at position " + nextSymbol + " doesn't match a (");//error if it is not there
                            MessageBox.Show("stop"); Application.Exit();
                        }
                    else
                        operatorStack[++operatorSP] = nextSymbol;//push operator on the stack
                }

                lastSymbol = program[nextSymbol].getToken();
            } // end for
        }

        int postfix2ast()
        // This is essentially a postfix evaluator:
        // operands are just pushed to a stack
        // operators pop the appropriate number of operands
        // and push the result (root of a subtree) back to the stack.
        {
        int[] evalStack = new int[1000];
        int evalSP=(-1);
        int i;

        for (i=0;i<postfixLength;i++)
          if (program[postfixIndex[i]].isIdentifier())
          {
              program[postfixIndex[i]].setleft(-1);
              program[postfixIndex[i]].setright(-1);
              
              ast[postfixIndex[i]].left=(-1);
            ast[postfixIndex[i]].right=(-1);
            evalStack[++evalSP]=postfixIndex[i];
          }
          else if (program[postfixIndex[i]].getToken() == "!")
          {

              program[postfixIndex[i]].setleft(evalStack[evalSP]);
              program[postfixIndex[i]].setright(-1);
              
              
              ast[postfixIndex[i]].left=evalStack[evalSP--];
            ast[postfixIndex[i]].right=(-1);
            evalStack[++evalSP]=postfixIndex[i];
          }
          else
          {
              program[postfixIndex[i]].setright(evalStack[evalSP]);
              program[postfixIndex[i]].setleft(evalStack[evalSP-1]);

              ast[postfixIndex[i]].right=evalStack[evalSP--];
              ast[postfixIndex[i]].left=evalStack[evalSP--];
              evalStack[++evalSP]=postfixIndex[i];

              



            if (program[postfixIndex[i]].getToken() == "?")
            {
                if (program[program[postfixIndex[i]].getleft()].getEvalToType() != "bool")
                {
                    MessageBox.Show("error in tree");
                }
            }
            else if (program[postfixIndex[i]].getToken() == ":")
            {
                if (program[program[postfixIndex[i]].getleft()].getEvalToType() != program[program[postfixIndex[i]].getright()].getEvalToType())
                {
                    MessageBox.Show("Conditional has conflicting types");
                }
            }
            else if (program[postfixIndex[i]].getEvalToType() == "numop")
            {
                if (program[program[postfixIndex[i]].getleft()].getEvalToType() == "int" && program[program[postfixIndex[i]].getright()].getEvalToType() == "int")
                {
                    MessageBox.Show("Trying to use numop on two non-ints");
                }
            }
            else if (program[postfixIndex[i]].getEvalToType() == "compop")
            {
                if (program[program[postfixIndex[i]].getleft()].getEvalToType() == "bool" && program[program[postfixIndex[i]].getright()].getEvalToType() == "bool")
                {
                    MessageBox.Show("Trying to use compop on two non-bools");
                }
            }
            if (program[postfixIndex[i]].getEvalToType() == null)
              program[postfixIndex[i]].setEvalType(program[program[postfixIndex[i]].getright()].getEvalToType());

          }

        return postfixIndex[postfixLength-1];
        }

        void printAST(int x, int indent)
        {
            int i;

            if (x == (-1))
                return;
            printAST(ast[x].right, indent + 1);
            for (i = 0; i < indent; i++)
                txtbxOutput.AppendText("  ");
            txtbxOutput.AppendText(program[x].getToken() + "  " + program[x].getOpType() + "  (" + x + ", " + ast[x].left + ", " + ast[x].right + ", " + program[x].getEvalToType() + ")\n");
            printAST(ast[x].left, indent + 1);
        }


        private void runIt()//ie MAIN()
        {
            program = new Token[1000];
            operatorStack = new int[2000]; // entries point to program[]
            operatorSP = -1;     // initial empty stac
            postfixIndex = new int[1000]; // Needed to build AST
            postfixLength = 0;
            waitingOperands = 0; // Indicates hypothetical unconsumed operands
            ast = new astType[1000];
            

            int i;
            int astRoot;

            readInput();//reads single line and verifies input

            translate();

            if (waitingOperands!=1)
              txtbxOutput.AppendText("Extra operands\n");
            if (operatorSP!=1)
              txtbxOutput.AppendText("Stack not empty\n");
            txtbxOutput.AppendText("Postfix: \n");
            for (i=0;i<postfixLength;i++)
                txtbxOutput.AppendText(program[postfixIndex[i]].getToken() + "  " + program[postfixIndex[i]].getOpType() + "\n");
            txtbxOutput.AppendText("\n");

            astRoot=postfix2ast();
            txtbxOutput.AppendText("AST:\n");
            printAST(astRoot,0);
        }
    }
}
