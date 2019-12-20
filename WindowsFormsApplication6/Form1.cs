using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using System.BitConverter;

namespace WindowsFormsApplication6
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        public MW_EasyPOD EasyPOD;

        unsafe public void btnGetUID_Click(object sender, EventArgs e)
        {           
            UInt32 drResult, dwResult, Index, uiWritten, uiRead;

            byte[] WriteBuffer_SN = new byte[] { 0x02, 0x01, 0x0D };//Command {STX, LEN, CMD,.....}
            byte[] sResponse_SN = null;
            sResponse_SN = new byte[10+2];


            EasyPOD.VID = 0xe6a;
            EasyPOD.PID = 0x317;
            Index = 1;
            

            fixed (MW_EasyPOD* pPOD = &EasyPOD)
            {

                dwResult = PODfuncs.ConnectPOD(pPOD, Index);

                if ((dwResult != 0))
                {
                    MessageBox.Show("Not connected yet");
                }
                else
                {
                    EasyPOD.ReadTimeOut = 200;
                    EasyPOD.WriteTimeOut = 200;

                    dwResult = PODfuncs.WriteData(pPOD, WriteBuffer_SN, Convert.ToUInt32(WriteBuffer_SN.Length), &uiWritten);
                    drResult = PODfuncs.ReadData(pPOD, sResponse_SN, Convert.ToUInt32(sResponse_SN.Length), &uiRead);

                    txbGetSNh.Text = BitConverter.ToString(sResponse_SN,4).Replace("-"," "); //HEX
                    txbGetSNt.Text = System.Text.Encoding.Default.GetString(sResponse_SN, 4, sResponse_SN.Length - 4);  //ASCII
                }
                dwResult = PODfuncs.ClearPODBuffer(pPOD);
                dwResult = PODfuncs.DisconnectPOD(pPOD);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
