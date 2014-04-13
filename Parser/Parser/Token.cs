using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Parser
{
    class Token
    {
        public static Dictionary<string, int> _bigDict= new Dictionary<string, int>
        {
            {"!",11},

            {"*",10},
            {"/",10},

            {"+",9},
            {"-",9},

            {"<", 8},
            {">", 8},
            {"<=",8},
            {">=",8},

            {"==", 7},

            {"&&",6},

            {"||",5},

            {"?",4},
            {":",4},

            {")",3},
            {"(",2},

            {"#",1},
            {"$",0},

        };

        public static Dictionary<string, string> operandDict = new Dictionary<string, string>();//dictionary is built at start of program

        public static void addOperand(string operand, string type)
        {
            operandDict.Add(operand,type);
        }

        public static void removeOperands()
        {
            operandDict.Clear();
        }


        private string token;
        private int precedence;
        private bool operand;
        private string opType;
        private string evalsTo;
        private int right;
        private int left;


        public Token(string token, string opType)
        {
            this.token = token;
            
            try
            {
                if (isName(this.token))
                {
                    try { operandDict.TryGetValue(this.token, out opType); }
                    catch (Exception e)
                    {
                        MessageBox.Show("bonk on input " + this.token + "\n");
                    }
                    operand = true;
                    evalsTo = opType;
                }
                else
                {
                    try { _bigDict.TryGetValue(this.token, out this.precedence); }
                    catch (Exception e)
                    {
                        MessageBox.Show("bonk on input " + this.token + "\n");
                        Application.Exit();
                    }
                    
                    operand = false;
                    evalsTo = null;

                }
                if (opType == "pic")
                {
                    operand = true;
                    evalsTo = "int";
                }
                if (opType == "compop")
                {
                    evalsTo = "bool";
                }
                if (opType == "numop")
                {
                    evalsTo = "int";
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }


        }

        public bool isIdentifier() { return operand; }//returns true if name,false if operator
        public string getToken() { return token; }
        public int getPrecedence() { return precedence; }
        public string getOpType() { return opType; }
        public string getEvalToType() { return evalsTo; }
        public void setright(int r) { right = r; }
        public void setleft(int l) { left = l; }
        public int getright() { return right; }
        public int getleft() { return left; }
        public void setEvalType(string evalType) { evalsTo = evalType; }

        public static bool isName(String match)
        {
            Boolean notWord = true;//assume not a word

            if (match.Length <= 10)
            {
                foreach (char c in match)
                {
                    if (Char.IsLetter(c))
                    {
                        if (Char.IsLower(c))
                        {
                            notWord = false;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return (notWord) ? false : true;
        }

        private static bool isNumber(String match)
        {
            long number = -1;
            try
            {
                number = long.Parse(match);
                if (number <= 1000000)
                    return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

    }
}
