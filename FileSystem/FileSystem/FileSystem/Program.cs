using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileSystem;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FileSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmFileSystem());

            //DiskController.CreateDisk(50000, "disk01");

            //string disk1 = "disk01";
            
            //byte[] beyonce = File.ReadAllBytes(@"C:\Users\Dave Cavaletto\Desktop\temp\a.jpg");
            //byte[] small11 = File.ReadAllBytes(@"C:\Users\Dave Cavaletto\Desktop\temp\small.pdf");
            ////byte[] nosave = small11;


            //bool one = DiskController.WriteFileToDisk(disk1, "a.jpg", beyonce);
            //bool two = DiskController.WriteFileToDisk(disk1, "small.pdf", small11);
            ////bool three = DiskController.WriteFileToDisk(disk1, "nosave.pdf", small11);

            //DiskController.DeleteFileFromDisk(disk1, "a.jpg");

            //bool four = DiskController.WriteFileToDisk(disk1, "small2.pdf", small11);

            //DiskController.WriteFileToDisk(disk1, "beyonce.jpg", beyonce);

            ////MessageBox.Show(one.ToString() + two.ToString() + three.ToString() + four.ToString());

            ////DiskController.PutFileToOS(disk1, "nosave.pdf", @"C:\Users\Dave Cavaletto\Desktop\temp");

            //DiskController.SaveFsAndUnMount(@"C:\Users\Dave Cavaletto\Desktop\temp", disk1);

            //DiskController.MountExistingDisk(@"C:\Users\Dave Cavaletto\Desktop\temp\disk01");


            //DiskController.PutFileToOS(disk1, "small.pdf", @"C:\Users\Dave Cavaletto\Desktop\temp\put");
            //DiskController.PutFileToOS(disk1, "small2.pdf", @"C:\Users\Dave Cavaletto\Desktop\temp\put");
            //DiskController.PutFileToOS(disk1, "beyonce.jpg", @"C:\Users\Dave Cavaletto\Desktop\temp\put");



            
            
            //testing reading and writing to a block
                    //string lessThan256 = "I can write some sentance here and store it and get it back";

                    //MessageBox.Show("in: " + lessThan256);

                    //System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
                    //Byte[] bytes = encoder.GetBytes(lessThan256);

                    //DataBlocks disk = new DataBlocks(10);

                    //disk.FormatBlock(0);
                    //disk.WriteBlock(0, bytes);

                    //byte[] read = disk.ReadBlock(0);
                    //string output = System.Text.Encoding.Default.GetString(read);
                    //MessageBox.Show("out: " + output);

                    //MessageBox.Show("in: " + lessThan256);

                    //disk.WriteBlock(disk.FindNextOpenBlock(), bytes);

                    //disk.WriteBlock(5, bytes);

                    //read = disk.ReadBlock(5);

                    //int max = disk.GetNumberFreeBlock();
                    //output = System.Text.Encoding.Default.GetString(read); 
                    //MessageBox.Show("out: " + output);

            //testing ABPT table
                    //ABPT t = new ABPT(10);

                    //t.WriteEntry(3, 4, DateTime.Now, 1, 2, 3, 4, 5, 6, 7, 8);

                    //t.WriteEntry(t.GetNextEmptyEntry(), 0, DateTime.Now);

                    //t.ClearEntry(3);

                    //t.FormatEntry(4);


           // Disk d = new Disk(25, "disk01");
           // Disk d2 = new Disk(50, "disk02");
           // string s = "A reader writes:  I’ve been a receptionist at a large reputable company for a few days shy of eight months. During my interview, I asked the recruiter why the position was available. She said that the previous receptionist wanted to be promoted within the company, but there wasn’t anything available, so she decided to resign. When I trained with the previous receptionist, she seemed pretty spiteful that she had to leave in order to advance her career. I don’t know her well enough to know, but this may or may not have been due to her work ethic; however, she also mentioned that the company has a hard time finding a good receptionist, so when they do, they try to keep them around and don’t want to promote them.  Now, there are a lot of things I love about this company. It’s in a great location; they’re really flexible with vacation time; they have a great benefits plan; I’m receiving an above-average wage for my role, and I’m never micromanaged. At first, the only thing I disliked about my job was that I am not allowed to leave my desk unless I call someone to cover my desk for me. I feel more guilty than I should when I have to ask for someone to cover my desk, because I’m interrupting their work. After less than a year, I’m starting to understand what the previous receptionist was feeling. I’m realizing that, despite my stellar performance review, I am not receiving the respect and high regard I feel I deserve. There are some people who treat me like nothing more than an order taker and in effect signal that they feel superior to me. I’ve spoken to my manager every time someone internal treats me this way, and she’s made efforts to curb their behaviour. Maybe I need to develop a thick skin to better cope with abrasive personalities, but I’m now beginning to realize that I could be doing much more with my life.  During my performance review, our HR rep and my manager both had great things to say about me, but they were complimenting my character traits more than my actual competencies. I know I have a lot more to offer than just being trustworthy and calm, but I don’t want to shell out all my talents for them to be incorporated into my present position. I made a point of telling our HR rep and my manager that although I like my job, I aspire to do more. Their response was, ‘Most people who start off in your position move up to another role with more responsibilities, so just be patient and we can talk about it in your next review (in eleven months).’ I’m weary to accept their response, because while it’s true that some people who started off in my position are still around, I know that a lot of previous receptionists have also resigned. Since the few people that started as receptionists now work in different offices, I don’t really have the opportunity to ask them what they did to move forward, and since I hardly know these people, I don’t feel comfortable emailing them out-of-the-blue to say ‘I’m unhappy with my job; how do I get yours?’  I’ve tried to ask for work from different departments in order to get some experience and prove that I can do more than keep calm and answer phones, but when I do I am often given some menial photocopying task rather than any challenging work. The truth is, I don’t want to be a receptionist for the rest of my life. For me, this is more of a means to an end, and now I’m more than ready to move on.  I guess I’m torn, because I want to stay loyal to the company but am not sure how much longer I need to wait to be promoted. Is it too early to feel it’s time to move up in the company? Given my restrictions (i.e., not being able to leave my desk, rarely being given meaningful work), do you have any suggestions on how I can prove to my manager and HR department that I’m ready for more responsibilities? And finally, if I don’t get what I want, do you suggest I hold out a little longer, or should I start looking more responsibility outside of the company?  Wow. You’ve only been there eight months. It’s pretty common to expect someone to stay in a job for a couple of years before getting promoted. So it strikes me as odd that you’re not just already looking to move up, but actually feeling bitter that you haven’t been given opportunities to move up yet. You’re still far, far away from the time when that would be warranted.  What’s more, they’ve even told you that it’s common in your position to move up, and that they’ll talk with you about it at your next performance review. And you even know that they’re not BSing you, because you’ve seen that there are other people who started in your job now working in higher-level jobs elsewhere in your company. So you don’t even need to wonder if this really happens; you can see that it does. But you’re still feeling angry. This is odd.  Basically, you’re working at a company in a great location with great benefits and flexible time off, getting paid an above-market salary, you’re not micromanaged, they promote from within, and you’ve been told that they’ll talk with you about advancement in the company in the not far-away future. But you’re angry anyway.  Your expectations are waaaayyy out of whack with reality. There’s so much wrong here that I’m just going to take some specific points in your letter one at a time:  'When I trained with the previous receptionist, she seemed pretty spiteful that she had to leave in order to advance her career.”  Yeah, sometimes this happens. Sometimes there aren’t any positions for someone to move into. In this case, it sounds like the company does like promoting from that role into others, so I’m going to guess that with the previous receptionist, either (a) nothing opened up on the timeline she wanted or (b) her work wasn’t good enough to make them interested in promoting her.  Speaking of which …  'I don’t want to shell out all my talents for them to be incorporated into my present position.”  This is how people move up. They show that they have additional value. If you decide to hold back in your current position, people may never think you’re capable of leaving it.  'The truth is, I don’t want to be a receptionist for the rest of my life. For me, this is more of a means to an end, and now I’m more than ready to move on.”  Yes, but you are a receptionist right now. That is the job that you accepted. And you’re only eight months into it.  Do you not see how whiny this sounds?  'Is it too early to feel it’s time to move up in the company?”  Yes.  'Given my restrictions (i.e., not being able to leave my desk, rarely being given meaningful work), do you have any suggestions on how I can prove to my manager and HR department that I’m ready for more responsibilities?”  Do your job, do it cheerfully, and believe them when they say that they’ll talk to you about advancement at your next review. And if you don’t find the work meaningful, you need to think about that. I assure you that the work is meaningful to them; that’s why they’re paying someone to do it.  'And finally, if I don’t get what I want, do you suggest I hold out a little longer, or should I start looking more responsibility outside of the company?”  I suggest you do your job for two years before you start feeling disgruntled that you haven’t moved up, and also that you radically revisit how you’re thinking about all this.  Right now, you’re not coming across like an employee anyone would want to have. You’re coming across as — I’m sorry — whiny, disconnected from your own choices, really high-maintenance, and pretty out of touch with reality  Yes, everyone wants intellectually challenging work. Yes, it’s frustrating when you’re in a job that bores you. But you are actually in an excellent situation compared to most people, and with what I’m betting is fairly light experience. That’s a good thing.  Step back and reconsider all of this.";



           // System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
           // Byte[] myfile = encoder.GetBytes(s);

           // MessageBox.Show("Will \"some string\" fit on disk01?: " + d.FileFitOnDisk(myfile).ToString());
           // MessageBox.Show("Will \"some string\" fit on disk02?: " + d2.FileFitOnDisk(myfile).ToString());

           // d2.WriteFileToFNT("mystring", d2.WriteFileToABPT(d2.WriteFile(myfile)));


           // d2.WriteFileToFNT("short string", d2.WriteFileToABPT(d2.WriteFile(encoder.GetBytes("a very short string"))));

           //// d2.DeleteFile(0);

           // d2.WriteFileToFNT("short string", d2.WriteFileToABPT(d2.WriteFile(encoder.GetBytes("a very short string"))));




           // IFormatter formatter = new BinaryFormatter();
           // Stream stream = new FileStream("C:\\myfolder\\d2.bin", FileMode.Create, FileAccess.Write, FileShare.None);
           // formatter.Serialize(stream, d2);
           // stream.Close();




           //  formatter = new BinaryFormatter();
           //  stream = new FileStream("C:\\myfolder\\d2.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
           // Disk obj = (Disk)formatter.Deserialize(stream);
            //stream.Close();


        }


    }
}
