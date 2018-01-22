using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HEX
{
    public partial class dm_hex : UserControl
    {
     private string _name;

        public string name {
            get { return _name; }
            set { _name = value;}
        }
        
        private byte[] _data=StrToByteArray("This is a really really empty array... or its an \"Empty Array\"");
        [DefaultValue(typeof(byte[]), "Empty")]
        public byte[] data {
            get { return _data; }
            set { _data = value; }
        }

        [DefaultValue(typeof(Color), "Black")]
        new public Color BackColor {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [DefaultValue(typeof(Color), "Green")]
        new public Color ForeColor {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        int _width = 16;
        [DefaultValue(typeof(int), "16")]
        public int data_width{
            get { return _width;}
            set { _width = value; }
        }

        private int _yBuffer;
        [DefaultValue (typeof(int), "0")]
        public int CharacterSpacing_Y{
            get { return _yBuffer; }
            set { _yBuffer=value; }
        }
        
        private int _xBuffer;
        [DefaultValue (typeof(int), "0")]
        public int CharacterSpacing_X{
            get { return _xBuffer; }
            set { _xBuffer=value; }
        }
        private int _ySpacer=2;
        [DefaultValue (typeof(int), "2")]
        public int LineSpacing{
            get { return _ySpacer; }
            set { _ySpacer=value; }
        }
        
        private int _xSpacer=8;
        [DefaultValue (typeof(int), "8")]
        public int GroupSpacing_X{
            get { return _xSpacer; }
            set { _xSpacer=value; }
        }

        private bool _drawAddress=true;
        [DefaultValue (typeof(bool), "TRUE")]
        public bool showAddress{
            get { return _drawAddress; }
            set { _drawAddress=value; }
        }

        
        

        public dm_hex() {
            InitializeComponent();
            brush = new SolidBrush(this.ForeColor);
            Graphics g=Graphics.FromImage(new Bitmap(10,10));
            charSize=g.MeasureString("0", font);

            charSize.Width-=2;
            charSize.Height-=2;            
        
        }



        protected override void OnPaint(PaintEventArgs pe) {
            Graphics g = pe.Graphics;
            createBackground(g);
        }        
        private void createBackground(Graphics g){
            
            drawBorder(g);
            int x = 0, y = 1;

            for (int a = 0; a < _width; a++) {
                drawHex(g,(byte)a,0,a);
            }
            
            if(_drawAddress) drawAddress(g,0);
            foreach (Byte b in _data) {
                drawHex  (g,b,y,x);
                drawAscii(g,b,y,x);
                x++;
                if (x == this.data_width) { 
                    x = 0; 
                    if(_drawAddress) drawAddress(g,y);
                    y++; 
                    
                }
            }
        }

        Font font = new System.Drawing.Font("FixedSys", 12, FontStyle.Regular);
        Brush brush;
        SizeF charSize;

        private void drawBorder(Graphics g){
            Color border=Color.FromArgb(0xff,0x77,0x77,0x77);
            
            Color cpen1=Color.FromArgb(       border.A,
                                        (int)(border.R*1),
                                        (int)(border.G*1),
                                        (int)(border.B*1));
            Color cpen2=Color.FromArgb(       border.A,
                                        (int)(border.R*0.80),
                                        (int)(border.G*0.80),
                                        (int)(border.B*0.80));
            
            Pen pen1=new Pen(cpen1);
            Pen pen2=new Pen(cpen2);
            int x1=0,x2=this.Width-1,y1=0,y2=this.Height-1;
            g.DrawLine(pen1,x1,y1,x2,y1);
            g.DrawLine(pen1,x1,y1,x1,y2);
            g.DrawLine(pen2,x2,y1,x2,y2);
            g.DrawLine(pen2,x1,y2,x2,y2);
        
            cpen1=Color.FromArgb(     border.A,
                                (int)(border.R*0.9),
                                (int)(border.G*0.90),
                                (int)(border.B*0.90));
            cpen2=Color.FromArgb(     border.A,
                                (int)(border.R*0.70),
                                (int)(border.G*0.70),
                                (int)(border.B*0.70));
            
            pen1=new Pen(cpen1);
            pen2=new Pen(cpen2);
        
            x1++; y1++; y2--; x2--;
            g.DrawLine(pen1,x1,y1,x2,y1);
            g.DrawLine(pen1,x1,y1,x1,y2);
            g.DrawLine(pen2,x2,y1,x2,y2);
            g.DrawLine(pen2,x1,y2,x2,y2);

        }
        private void drawAddress(Graphics g,int row){
            int address=(row)*_width;
            Point point = new Point();

            point.X=0;
            point.Y=(row+1)   *((int)charSize.Height+_yBuffer)+_ySpacer*(row+1);
            string s = String.Format("{0:X6}", address);
            
            g.DrawString(s, font, brush, point);
        }

        private void drawBox(Graphics g,float x1,float y1,float x2,float y2){
            drawBox(g,(int)x1,(int)y1,(int)x2,(int)y2);
        }

        private void drawBox(Graphics g,Color bg,int x1,int y1,int x2,int y2){
            Brush brush=new SolidBrush(bg);
            g.FillRectangle(brush, x1,y1,x2-x1,y2-y1);
            /*g.DrawLine(pen,x1,y1,x2,y1);
            g.DrawLine(pen,x1,y1,x1,y2);
            g.DrawLine(pen,x2,y1,x2,y2);
            g.DrawLine(pen,x1,y2,x2,y2);*/
        }

            
        private void drawHex(Graphics g ,byte b,int row, int column){
            Point point = new Point();
            
            Color bg=this.BackColor;
            if((b>='A' && b<='Z') ||
               (b>='a' && b<='z') ||
               (b>='0' && b<='9') ||
               b=='!' ||
               b=='@' ||
               b=='#' ||
               b=='$' ||
               b=='%' ||
               b=='^' ||
               b=='&' ||
               b=='(' ||
               b==')' ||
               b=='-' ||
               b=='+' ||
               b=='=' ||
               b=='_' ||
               b==' ' ||
               b=='\'' ||
               b=='"' ) {
                 bg=Color.Orange;
            }

            int left=0;
            if(_drawAddress) left=(int)charSize.Width*5+_xSpacer;

            int _x=column*((int)charSize.Width +_xBuffer)*2+_xSpacer*column+left;
            int _y=row   *((int)charSize.Height+_yBuffer)+_ySpacer*row;
            string s = String.Format("{0:X}", b>>4);
            SizeF charSize2=g.MeasureString(s, font);
            point.X = _x +(int)(charSize.Width-charSize2.Width)/2;;
            point.Y = _y ;
            if(this.BackColor!=bg) {
                drawBox(g,bg,_x,_y,_x+((int)charSize.Width +_xBuffer),_y+(int)charSize.Height +_yBuffer);
            }
            g.DrawString(s, font, brush, point);

            _x+=((int)charSize.Width +_xBuffer);
            s = String.Format("{0:X}", b%0x10);
            charSize2=g.MeasureString(s, font);
            point.X = _x +(int)(charSize.Width-charSize2.Width)/2;
            point.Y = _y ;
            if(this.BackColor!=bg) {
                drawBox(g,bg,_x,_y,_x+((int)charSize.Width +_xBuffer),_y+(int)charSize.Height +_yBuffer);
            }
            g.DrawString(s, font, brush, point);
        }

        private void drawAscii(Graphics g ,byte b,int row, int column){
            Point point = new Point();

            int left=0;
            if(_drawAddress) left=(int)charSize.Width*5+_xSpacer;

            int _x=data_width*((int)charSize.Width +_xBuffer)*2+_xSpacer*data_width+left +column*((int)charSize.Width +_xBuffer)+20;
            int _y=row   *((int)charSize.Height+_yBuffer)+_ySpacer*row;
            string s =".";
            
            if((b>='A' && b<='Z') ||
               (b>='a' && b<='z') ||
               (b>='0' && b<='9') ||
               b=='!' ||
               b=='@' ||
               b=='#' ||
               b=='$' ||
               b=='%' ||
               b=='^' ||
               b=='&' ||
               b=='(' ||
               b==')' ||
               b=='-' ||
               b=='+' ||
               b=='=' ||
               b=='\'' ||
               b=='"' ||
               b=='_' ) {
                 s=Char.ConvertFromUtf32(b);
            }
               
            SizeF charSize2=g.MeasureString(s, font);
            point.X = _x +(int)(charSize.Width-charSize2.Width)/2;;
            point.Y = _y ;
            //drawBox(g,_x,_y,_x+((int)charSize.Width +_xBuffer),_y+(int)charSize.Height +_yBuffer);
            g.DrawString(s, font, brush, point);
        }


            // C# to convert a string to a byte array.
            public static byte[] StrToByteArray(string str) {
                System.Text.UTF8Encoding  encoding=new System.Text.UTF8Encoding();
                return encoding.GetBytes(str);
            }
 
    }
}
