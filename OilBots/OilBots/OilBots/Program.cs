using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OilBots;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class EcoSystem : Form
{
    public static Boolean cbox;
    private Timer timer;
    private Swarm swarm;
    private Image iconRegular;
    private static int boundary = 800;
    public static Bitmap background = new Bitmap("C:\\Users\\Dave Cavaletto\\Documents\\Visual Studio 2010\\Projects\\OilBots\\blob.png");



    public void changeBackground()
    {
        if (cbox)
        {
            EcoSystem.background = new Bitmap("C:\\Users\\Dave Cavaletto\\Documents\\image.png");
            BackgroundImage = background;
            BackgroundImageLayout = ImageLayout.Stretch;
        }
        else
        {
            EcoSystem.background = new Bitmap("C:\\Users\\Dave Cavaletto\\Documents\\whiteonly.bmp");
            BackgroundImage = background;
            BackgroundImageLayout = ImageLayout.Stretch;
        }
    }

    public EcoSystem()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        BackgroundImage = background;
        BackgroundImageLayout = ImageLayout.Stretch;
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(boundary, boundary);
        iconRegular = CreateIcon(Pens.Blue);
        swarm = new Swarm(boundary); 
        timer = new Timer();
        timer.Tick += new EventHandler(this.timer_Tick);
        timer.Interval = 75;
        timer.Start();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        foreach (Robot Robot in Swarm.swarm)
        {
            float angle;
            if (Robot.dX == 0) angle = 90f;
            else angle = (float)(Math.Atan(Robot.dY / Robot.dX) * 57.3);
            if (Robot.dX < 0f) angle += 180f;
            Matrix matrix = new Matrix();
            matrix.RotateAt(angle, Robot.Position);
            e.Graphics.Transform = matrix;
            e.Graphics.DrawImage(iconRegular, Robot.Position);
        }
    }

    public static Bitmap getBackground()
    {
        return background;
    }

    public static void updateEco(int size)
    {
        boundary = size;
    }

    public static Bitmap ResizeBitmap(Bitmap b, int nWidth, int nHeight)
    {
        Bitmap result = new Bitmap(nWidth, nHeight);
        using (Graphics g = Graphics.FromImage((Image)result))
            g.DrawImage(b, 0, 0, nWidth, nHeight);
        return result;
    }

    private static Image CreateIcon(Pen pen)
    {

        Bitmap icon = new Bitmap(10,10);
        Graphics g = Graphics.FromImage(icon);
        Rectangle rect = new Rectangle(0, 0, 10, 10);
        System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
        //g.DrawRectangle(pen, rect);
        g.FillRectangle(myBrush, rect);
        //g.DrawEllipse(pen, 0, 0, 10, 10);
        //g.FillPolygon(pen, points);
        return icon;
    }

    private void timer_Tick(object sender, EventArgs e)
    {
        swarm.MoveRobots();
        Invalidate();
    }

    public Swarm getSwarm()
    {
        return swarm;
    }
}

public class Swarm
{
    public static List<Robot> swarm = new List<Robot>();
    private static int numBots;  //number of robots in the simulation------------------------------------------
    private static float space;

    public Swarm(int boundary)
    {
        for (int i = 0; i < numBots; i++)
        {
            space = (float)( (boundary / Math.Sqrt(numBots) )*.8 );
            swarm.Add(new Robot(boundary,space ));//determine the space between the robots based on boundary size and robot number
        }
    }

    public void MoveRobots()
    {
        foreach (Robot Robot in swarm)
        {
            Robot.move(swarm);
        }
    }

    public static void updateBots(int bots)//gets the number of bots from the Opening Form
    {
        numBots = bots;
    }
}

public class Robot
{

    private static Random rnd = new Random();
    private static float border;
    private static float space;   //1/5 of boundry size?
    private static float speed = 10;
    private static int idgen = 0;
    public Boolean oil;
    private float sight; 
    private int ID;
    private float boundary;
    public float dX;
    public float dY;
    public PointF Position;
    //history struct and related indices
    public struct hist
    {
        public PointF position;
        public float percent;
    }
    private hist[] posHistory = new hist[5];
    int histPoint = 0;//index for storing history item in array

    #region methods

    public Robot(int boundary)
    {
        Position = new PointF(rnd.Next(boundary), rnd.Next(boundary));
        this.boundary = boundary;
    }

    public Robot(int boundary, float ispace)
    {
        ID = idgen++;
        Position = new PointF(rnd.Next(boundary), rnd.Next(boundary));
        this.boundary = boundary;
        space = ispace;
        sight = space + 50;
        border = boundary * .1f;
    }

   
    
    public void move(List<Robot> swarm)
    {
        
        Boolean run = false;
        Boolean oil= false;
        PointF OG = Position;
        foreach (Robot robot in swarm)
        {
            float distance = Distance(Position, robot.Position);//the distance between this robot and each other robot
            float edistance = boundary - border;
            recordHist();//record my position and oil percentage

            if (checkOilPercent() > 35 && checkOilPercent() < 65)
            {
                run = true;
                oil = true;
                robot.oil = true;
                
                if (robot != this && robot.checkOil())
                {
                    //move or something
                    dX += (Position.X - robot.Position.X) / 400;
                    dY += (Position.Y - robot.Position.Y) / 400;
                }
            }
            else
            {
                if (robot != this && distance < sight && !robot.checkOil())
                {//not looking at the current robot and distance between this and the other robot is < sight and this robot has not found oil
                    run = true;
                    if (distance < space)//that robot is in my space, move away
                    {
                        dX += (Position.X - robot.Position.X) / 400;
                        dY += (Position.Y - robot.Position.Y) / 400;
                    }
                    else //that robot is in my sight, but out of my space, don't do anything
                    {
                        dX += 0;
                        dY += 0;
                    }
                }
                /*if (!run)//if a robot is not in sight of any other robots, move towards the center of the map
                {
                    dX -=  (Position.X - 200) / 400;
                    dY -=  (Position.Y - 200) / 400;
                }*/
            }
        }
        if (!oil)
        {
            CheckBounds();//verify bounds
            CheckSpeed();
            Position.X += dX;
            Position.Y += dY;
        }
    }

    private void recordHist()
    {
        posHistory[histPoint].percent = checkOilPercent();//store my oil  percentage
        posHistory[histPoint].position = Position;//store my position
        //Console.WriteLine("per: " + posHistory[histPoint].percent.ToString() + " pos:" + posHistory[histPoint].position.ToString());
        histPoint++;
        if (histPoint > 4)
            histPoint = 0;//store last 5 movements
    }
    private static float Distance(PointF p1, PointF p2)
    {
        double val = Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2);
        return (float)Math.Sqrt(val);
    }

    private void CheckBounds()
    {
        float val = boundary - border;
        if (Position.X < border) dX += border - Position.X; //if bot crosses bottom or left border, bot jumps back to border
        if (Position.Y < border) dY += border - Position.Y;
        if (Position.X > val) dX += val - Position.X;       //if bot crosses right or top border, bot jumps back to border line
        if (Position.Y > val) dY += val - Position.Y;
    }

    private void CheckSpeed()
    {
        float s;
        s = speed / 4f;
        float val = Distance(new PointF(0f, 0f), new PointF(dX, dY));
        if (val > s)
        {
            dX = dX * s / val;
            dY = dY * s / val;
        }
    }

    private Boolean checkOil()
    {
        int a = 0, b = 0;
        Color color;
        Bitmap bkgrnd = EcoSystem.getBackground();
        for (a = 0; a < 10; a++)
            for (b = 0; b < 10; b++)
            {
                try
                {
                    color = bkgrnd.GetPixel((int)Position.X + a, (int)Position.Y);
                    if ((color.R == 0) && (color.G == 0) && (color.G == 0) && (color.A == 255))//if any black pixel...
                        return true;
                }
                catch { }
            }
        return false;
    }

    private int checkOilPercent()//returns portion of the square that is white
    {
        int a = 0, b = 0, black=0, white=0;
        Color color;
        Bitmap bkgrnd = EcoSystem.getBackground();

        for (a = 0; a < 10; a++)
            for (b = 0; b < 10; b++)
            {
                try 
                { 
                    color = bkgrnd.GetPixel((int)Position.X + a, (int)Position.Y + b); 
                
                    if ((color.R == 0) && (color.G == 0) && (color.B == 0) && (color.A == 255))//black
                        black++;
                    else
                        white++;
                }
                catch { }
            }
       
        return black ;
    }


    #endregion methods
}

