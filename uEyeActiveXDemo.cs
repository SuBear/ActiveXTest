//==========================================================================//
//                                                                          //
//  Copyright (C) 2004 - 2012                                               //
//  IDS Imaging GmbH                                                        //
//  Dimbacherstr. 6-8                                                       //
//  D-74182 Obersulm-Willsbach                                              //
//                                                                          //
//  The information in this document is subject to change without           //
//  notice and should not be construed as a commitment by IDS Imaging GmbH. //
//  IDS Imaging GmbH does not assume any responsibility for any errors      //
//  that may appear in this document.                                       //
//                                                                          //
//  This document, or source code, is provided solely as an example         //
//  of how to utilize IDS software libraries in a sample application.       //
//  IDS Imaging GmbH does not assume any responsibility for the use or      //
//  reliability of any portion of this document or the described software.  //
//                                                                          //
//  General permission to copy or modify, but not for profit, is hereby     //
//  granted,  provided that the above copyright notice is included and      //
//  reference made to the fact that reproduction privileges were granted	//
//	by IDS Imaging GmbH.				                                    //
//                                                                          //
//  IDS cannot assume any responsibility for the use or misuse of any       //
//  portion of this software for other than its intended diagnostic purpose	//
//  in calibrating and testing IDS manufactured cameras and software.		//
//                                                                          //
//==========================================================================//

using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data;
using AxuEyeCamLib;
using Timer = System.Windows.Forms.Timer;
 

namespace uEyeActiveXDemo_CS
{
	/// <summary>
	/// Zusammenfassung für Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button Init;
		private System.Windows.Forms.Button Properties;
		private System.Windows.Forms.Button triggerVideo;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton TriggerSoftware;
		private System.Windows.Forms.RadioButton TriggerFalling;
		private System.Windows.Forms.RadioButton TriggerRising;
		private System.Windows.Forms.TextBox Info1;
		private System.Windows.Forms.TextBox Info2;
		private System.Windows.Forms.TextBox Info3;
		private System.Windows.Forms.ListBox InfoList;
		private bool m_bCameraOpen;
		private bool m_bLive;
		private Int64 m_MemAddr;
		private byte[] rgb;
        private bool m_bContinuousTriggerSupported;
        private int m_nCurrentMode;
        private int m_nTransferFailed;
        private int m_nTransferFailedOld;
        private Timer t1;
        
        private System.Drawing.Point LastPoint;
		private System.Windows.Forms.RadioButton TriggerOff;
		private System.Windows.Forms.Button CaptureImage;
        private System.Windows.Forms.CheckBox Direct3D;
        private AxuEyeCamLib.AxuEyeCam axuEyeCam;

	    private string _triggerSaveFolderPath;
	    private int _timesCameraTriggered = 0;
	    private int _secondsPassed = 0;
	    private bool _imagesAreBeingSaved;

	    private readonly Timer _timer = new Timer();
        		
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();
			m_bCameraOpen = false;
            rgb = new byte[3];
            m_MemAddr = 0;
			LastPoint = new System.Drawing.Point(0, 0);
            m_nCurrentMode = 0;
            m_nTransferFailed = 0;
            m_nTransferFailedOld = 0;

            t1 = new Timer();
            t1.Interval = 100;
            t1.Tick += new EventHandler(t1_Tick);
            t1.Start();

		    _timer.Interval = 10000;
            _timer.Tick += _timer_Tick;
		}

        private void _timer_Tick(object sender, EventArgs e)
        {
            _secondsPassed++;
            Debug.WriteLine(string.Format("{0} seconds passed", _secondsPassed * 10));
        }

	    public AxuEyeCam GetAxuEyeCam
	    {
            get { return axuEyeCam; }
	    }

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Init = new System.Windows.Forms.Button();
            this.triggerVideo = new Button();
            this.Properties = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TriggerOff = new System.Windows.Forms.RadioButton();
            this.TriggerRising = new System.Windows.Forms.RadioButton();
            this.TriggerSoftware = new System.Windows.Forms.RadioButton();
            this.TriggerFalling = new System.Windows.Forms.RadioButton();
            this.Info1 = new System.Windows.Forms.TextBox();
            this.Info2 = new System.Windows.Forms.TextBox();
            this.Info3 = new System.Windows.Forms.TextBox();
            this.InfoList = new System.Windows.Forms.ListBox();
            this.Direct3D = new System.Windows.Forms.CheckBox();
            this.axuEyeCam = new AxuEyeCamLib.AxuEyeCam();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axuEyeCam)).BeginInit();
            this.SuspendLayout();
            // 
            // Init
            // 
            this.Init.Location = new System.Drawing.Point(8, 8);
            this.Init.Name = "Init";
            this.Init.Size = new System.Drawing.Size(128, 24);
            this.Init.TabIndex = 0;
            this.Init.Text = "Init Camera";
            this.Init.Click += new System.EventHandler(this.Init_Click);
            // 
            // triggerVideo
            // 
            this.triggerVideo.Location = new System.Drawing.Point(8, 120);
            this.triggerVideo.Name = "triggerVideo";
            this.triggerVideo.Size = new System.Drawing.Size(128, 24);
            this.triggerVideo.TabIndex = 1;
            this.triggerVideo.Text = "Start Saving Images";
            this.triggerVideo.Click += new System.EventHandler(this.triggerVideo_Click);
            // 
            // Properties
            // 
            this.Properties.Location = new System.Drawing.Point(8, 388);
            this.Properties.Name = "Properties";
            this.Properties.Size = new System.Drawing.Size(128, 24);
            this.Properties.TabIndex = 11;
            this.Properties.Text = "Properties";
            this.Properties.Click += new System.EventHandler(this.Properties_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TriggerOff);
            this.groupBox1.Controls.Add(this.TriggerRising);
            this.groupBox1.Controls.Add(this.TriggerSoftware);
            this.groupBox1.Controls.Add(this.TriggerFalling);
            this.groupBox1.Location = new System.Drawing.Point(8, 167);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(128, 117);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Trigger mode";
            // 
            // TriggerOff
            // 
            this.TriggerOff.Location = new System.Drawing.Point(12, 24);
            this.TriggerOff.Name = "TriggerOff";
            this.TriggerOff.Size = new System.Drawing.Size(88, 16);
            this.TriggerOff.TabIndex = 6;
            this.TriggerOff.Text = "Off";
            this.TriggerOff.Click += new System.EventHandler(this.TriggerOff_Click);
            // 
            // TriggerRising
            // 
            this.TriggerRising.Location = new System.Drawing.Point(12, 93);
            this.TriggerRising.Name = "TriggerRising";
            this.TriggerRising.Size = new System.Drawing.Size(88, 16);
            this.TriggerRising.TabIndex = 9;
            this.TriggerRising.Text = "Rising edge";
            this.TriggerRising.Click += new System.EventHandler(this.TriggerRising_Click);
            // 
            // TriggerSoftware
            // 
            this.TriggerSoftware.Location = new System.Drawing.Point(12, 47);
            this.TriggerSoftware.Name = "TriggerSoftware";
            this.TriggerSoftware.Size = new System.Drawing.Size(80, 16);
            this.TriggerSoftware.TabIndex = 7;
            this.TriggerSoftware.Text = "Software";
            this.TriggerSoftware.Click += new System.EventHandler(this.TriggerSoftware_Click);
            // 
            // TriggerFalling
            // 
            this.TriggerFalling.Location = new System.Drawing.Point(12, 70);
            this.TriggerFalling.Name = "TriggerFalling";
            this.TriggerFalling.Size = new System.Drawing.Size(96, 16);
            this.TriggerFalling.TabIndex = 8;
            this.TriggerFalling.Text = "Falling edge";
            this.TriggerFalling.Click += new System.EventHandler(this.TriggerFalling_Click);
            // 
            // Info1
            // 
            this.Info1.Location = new System.Drawing.Point(144, 370);
            this.Info1.Name = "Info1";
            this.Info1.ReadOnly = true;
            this.Info1.Size = new System.Drawing.Size(176, 20);
            this.Info1.TabIndex = 9;
            this.Info1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Info2
            // 
            this.Info2.Location = new System.Drawing.Point(320, 370);
            this.Info2.Name = "Info2";
            this.Info2.ReadOnly = true;
            this.Info2.Size = new System.Drawing.Size(224, 20);
            this.Info2.TabIndex = 10;
            this.Info2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Info3
            // 
            this.Info3.Location = new System.Drawing.Point(544, 370);
            this.Info3.Name = "Info3";
            this.Info3.ReadOnly = true;
            this.Info3.Size = new System.Drawing.Size(80, 20);
            this.Info3.TabIndex = 15;
            this.Info3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // InfoList
            // 
            this.InfoList.Location = new System.Drawing.Point(8, 288);
            this.InfoList.Name = "InfoList";
            this.InfoList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.InfoList.Size = new System.Drawing.Size(128, 95);
            this.InfoList.TabIndex = 10;
            // 
            // Direct3D
            // 
            this.Direct3D.Location = new System.Drawing.Point(20, 143);
            this.Direct3D.Name = "Direct3D";
            this.Direct3D.Size = new System.Drawing.Size(104, 20);
            this.Direct3D.TabIndex = 4;
            this.Direct3D.Text = "use Direct3D";
            this.Direct3D.CheckedChanged += new System.EventHandler(this.Direct3D_CheckedChanged);
            // 
            // axuEyeCam
            // 
            this.axuEyeCam.Enabled = true;
            this.axuEyeCam.Location = new System.Drawing.Point(144, 8);
            this.axuEyeCam.Name = "axuEyeCam";
            this.axuEyeCam.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axuEyeCam.OcxState")));
            this.axuEyeCam.Size = new System.Drawing.Size(480, 355);
            this.axuEyeCam.TabIndex = 16;
		    //this.axuEyeCam.EventOnTrigger += axuEyeCam_EventOnTrigger;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(632, 426);
            this.Controls.Add(this.axuEyeCam);
            this.Controls.Add(this.triggerVideo);
            this.Controls.Add(this.Direct3D);
            this.Controls.Add(this.Info3);
            this.Controls.Add(this.Info2);
            this.Controls.Add(this.Info1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Properties);
            this.Controls.Add(this.CaptureImage);
            this.Controls.Add(this.Init);
            this.Controls.Add(this.InfoList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "IDS uEye ActiveX";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axuEyeCam)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// Der Haupteinstiegspunkt für die Anwendung.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			UpdateControls();
		}

		private void Form1_Closing(object sender, CancelEventArgs e)
		{
			axuEyeCam.ExitCamera();
		}

		private void UpdateControls()
		{
			Properties.Enabled = m_bCameraOpen;
		    triggerVideo.Enabled = m_bCameraOpen;
			TriggerOff.Enabled = m_bCameraOpen;

            bool bTriggerSoftware = false;
            bool bTriggerLoHi = false;
            bool bTriggerHiLo = false;
            m_bContinuousTriggerSupported = false;

            if (m_bCameraOpen)
            {
                int nSupportedTriggerMode = axuEyeCam.SetExternalTrigger(uEye_defines.IS_GET_SUPPORTED_TRIGGER_MODE);
                if ((nSupportedTriggerMode & uEye_defines.IS_SET_TRIG_SOFTWARE) == uEye_defines.IS_SET_TRIG_SOFTWARE)
                {
                    bTriggerSoftware = true;
                }

                if ((nSupportedTriggerMode & uEye_defines.IS_SET_TRIG_LO_HI) == uEye_defines.IS_SET_TRIG_LO_HI)
                {
                    bTriggerLoHi = true;
                }

                if ((nSupportedTriggerMode & uEye_defines.IS_SET_TRIG_HI_LO) == uEye_defines.IS_SET_TRIG_HI_LO)
                {
                    bTriggerHiLo = true;
                }

                if ((nSupportedTriggerMode & uEye_defines.IS_SET_TRIGGER_CONTINUOUS) == uEye_defines.IS_SET_TRIGGER_CONTINUOUS)
                {
                    m_bContinuousTriggerSupported = true;
                }
            }

            TriggerSoftware.Enabled = m_bCameraOpen & bTriggerSoftware;
            TriggerFalling.Enabled = m_bCameraOpen & bTriggerHiLo;
            TriggerRising.Enabled = m_bCameraOpen & bTriggerLoHi;
			
			if( m_bCameraOpen )
			{
				Init.Text = "Exit Camera";
				Info1.Text = String.Format("{0} SerNo:{1}", axuEyeCam.GetSensorName(), axuEyeCam.GetSerialNumber());
				Info2.Text = String.Format("{0} x {1}", axuEyeCam.GetImageWidth().ToString(), axuEyeCam.GetImageHeight().ToString() );
				Direct3D.Checked = axuEyeCam.EnableDirect3D;
			}
			else
			{
				Init.Text = "Init Camera";
				Info1.Text = String.Format("No open camera");
				Info2.Text = "-";
				Info3.Text = "0 fps";

				TriggerOff.Checked = true;
			}

			if( axuEyeCam.EnableDirect3D )
			{
				Info2.Text = "Click to draw a cross!";
			}
		}

		private void Init_Click(object sender, System.EventArgs e)
        {
            m_MemAddr = 0;
			if(m_bCameraOpen)
			{
				axuEyeCam.ExitCamera();

				m_bCameraOpen = false;
				m_bLive = false;

				// Disable events
				axuEyeCam.EnableEvent( uEye_defines.IS_FRAME, 0 );	
				axuEyeCam.EnableEvent( uEye_defines.IS_DEVICE_REMOVED, 0 );	
				axuEyeCam.EnableEvent( uEye_defines.IS_TRANSFER_FAILED, 0 );
				axuEyeCam.EnableEvent( uEye_defines.IS_TRIGGER, 0 );

                axuEyeCam.EventOnFrame -= axuEyeCam_EventOnFrame;
                axuEyeCam.EventOnTransferFailed -= axuEyeCam_EventOnTransferFailed;
                axuEyeCam.MouseMoveEvent -= axuEyeCam_MouseMoveEvent;
                axuEyeCam.MouseDownEvent -= axuEyeCam_MouseDownEvent;
			}
			else
			{
				int nRet = axuEyeCam.InitCamera(0);
				if (nRet == uEye_defines.IS_STARTER_FW_UPLOAD_NEEDED)
				{
					int nUploadTime = 0;
					axuEyeCam.GetDuration (uEye_defines.IS_STARTER_FW_UPLOAD, ref nUploadTime);

					String Str;
					Str = "This camera requires a new firmware. The upload will take about " + (nUploadTime / 1000).ToString() + " seconds.";

					MessageBox.Show(Str);
	
					nRet = axuEyeCam.InitCamera(0 | uEye_defines.IS_ALLOW_STARTER_FW_UPLOAD);
				}

				if (nRet == uEye_defines.IS_SUCCESS)
				{				
					m_bCameraOpen = true;
					m_bLive = true;

					// Enable events
					axuEyeCam.EnableEvent( uEye_defines.IS_FRAME, 1 );	
					axuEyeCam.EnableEvent( uEye_defines.IS_DEVICE_REMOVED, 1 );	
					axuEyeCam.EnableEvent( uEye_defines.IS_TRANSFER_FAILED, 1 );
					axuEyeCam.EnableEvent( uEye_defines.IS_TRIGGER, 1 );

                    axuEyeCam.EventOnFrame += axuEyeCam_EventOnFrame;
                    axuEyeCam.EventOnTransferFailed += axuEyeCam_EventOnTransferFailed;
                    axuEyeCam.MouseMoveEvent += axuEyeCam_MouseMoveEvent;
                    axuEyeCam.MouseDownEvent += axuEyeCam_MouseDownEvent;

                    m_nTransferFailed = 0;
                    m_nTransferFailedOld = 0;
                    InfoList.Items.Clear();
					InfoList.Items.Add( String.Format( "{0} opened!", axuEyeCam.GetSensorName() ));

                    // Set the trigger timeout to 5 s (500 = 5000 ms)
                    axuEyeCam.SetTimeout(uEye_defines.IS_TRIGGER_TIMEOUT, 500);
				}
				else
				{
					MessageBox.Show("Init failed","Could not open camera!");
				}
			}

			UpdateControls( );
		}

		private void Properties_Click(object sender, System.EventArgs e)
		{
			axuEyeCam.PropertyDialog();
			UpdateControls();
		}

        private void triggerVideo_Click(object sender, System.EventArgs e)
        {
            if (!_imagesAreBeingSaved)
            {
                _triggerSaveFolderPath = string.Empty;
                using (var dialog = new FolderBrowserDialog())
                {
                    dialog.Description = "Folder to save trigger";
                    dialog.ShowNewFolderButton = false;
                    dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        _timer.Start();
                        _imagesAreBeingSaved = true;
                        triggerVideo.Text = "Stop Saving Images";
                        //axuEyeCam.EventOnTrigger += axuEyeCam_EventOnTrigger;
                        _triggerSaveFolderPath = dialog.SelectedPath;
                        StartSavingImages();
                        //TriggerRising.PerformClick();
                    }
                }
            }
            else
            {
                _timer.Stop();
                _imagesAreBeingSaved = false;
                triggerVideo.Text = "Start Saving Images";
                //axuEyeCam.EventOnTrigger -= axuEyeCam_EventOnTrigger;
                //TriggerOff.PerformClick();
            }
        }

        private void StartSavingImages()
        {
            new Thread(() =>
                {
                    while (_imagesAreBeingSaved)
                    {
                        _timesCameraTriggered++;
                        axuEyeCam.SaveImageEx(_triggerSaveFolderPath + "\\" + _timesCameraTriggered + ".jpg", 1, 0);
                    }
                }).Start();
            
        }

		private void Direct3D_CheckedChanged(object sender, System.EventArgs e)
		{
			axuEyeCam.EnableDirect3D = Direct3D.Checked;
			UpdateControls();
		}

		private void axuEyeCam_EventOnTrigger(object sender, System.EventArgs e)
		{
            axuEyeCam.SaveImageEx(_triggerSaveFolderPath + "\\" + _timesCameraTriggered + ".jpg", 1, 0);
		}

		private void axuEyeCam_EventOnTransferFailed(object sender, System.EventArgs e)
		{
            m_nTransferFailed++;
        }

        void t1_Tick(object sender, EventArgs e)
        {
            if (m_bCameraOpen && (m_nTransferFailedOld != m_nTransferFailed))
            {
                String str;
                str = "Transfer failed: " + m_nTransferFailed;
                InfoList.Items.Add(str);

                m_nTransferFailedOld = m_nTransferFailed;
            }
        }

        private delegate void UpdateFPS();
        private void ShowFPS()
        {
            Info3.Text = String.Format(" {0} fps ", axuEyeCam.GetFramesPerSecond().ToString().Substring(0, 5));
        }

        private void axuEyeCam_EventOnFrame(object sender, AxuEyeCamLib._DuEyeCamEvents_EventOnFrameEvent e)
        {
            Info3.Invoke(new UpdateFPS(ShowFPS));
            m_MemAddr = System.Convert.ToInt64(axuEyeCam.GetImageMem());
        }

		private void axuEyeCam_MouseMoveEvent(object sender, AxuEyeCamLib._DuEyeCamEvents_MouseMoveEvent e)
		{
			if( m_bCameraOpen && !axuEyeCam.EnableDirect3D )
			{	
				int x = 0, y = 0;
				int width = 0, height = 0, bits = 0, pitch = 0;
				axuEyeCam.InquireImageMem( ref width, ref height, ref bits, ref pitch );

				if( axuEyeCam.InitFitToWindow )
				{
					x = e.x * width / axuEyeCam.Size.Width;
					y = e.y * height / axuEyeCam.Size.Height;
				}
				else
				{
					x = e.x;
					y = e.y;
                }
                if( (x < 0) || (x > width) || (y < 0) || (y > height) )
                {
                    Info2.Text = String.Format("Invalid position");
                    return;	
                }
                
                if (this.m_MemAddr != 0)
                {                
				    // try to get the rgb values (in case of 24 or 32 bit images)
				    // on Y8 images you got 3 Y values
				    try
    				{   
	    				rgb[0] = Marshal.ReadByte( (IntPtr)this.m_MemAddr, (y*width + x)*bits/8 );
		    			rgb[1] = Marshal.ReadByte( (IntPtr)this.m_MemAddr, (y*width + x)*bits/8 + 1 );
			    		rgb[2] = Marshal.ReadByte( (IntPtr)this.m_MemAddr, (y*width + x)*bits/8 + 2 );
    				}
	    			catch(Exception ee)
		    		{
			    		Console.WriteLine("ScanForm : " + ee.Message + " Type : " + ee.GetType().ToString());
                    }		    		
                    Info2.Text = String.Format(" ({0},{1}) = ({2},{3},{4})", x, y, rgb[0], rgb[1], rgb[2] );
                }   // end if (this.m_MemAddr != 0)
                else
                {
                    Info2.Text = String.Format(" ({0},{1}) = (N/A)", x, y);
                }
			}
		}

		private void axuEyeCam_MouseDownEvent(object sender, AxuEyeCamLib._DuEyeCamEvents_MouseDownEvent e)
		{
			if( axuEyeCam.EnableDirect3D && m_bCameraOpen )
			{
				System.Drawing.Point p = new System.Drawing.Point( e.x, e.y );
				DrawCross (p);
				axuEyeCam.ShowDirect3DOverlay = true;
			}		
		}

		private void DrawCross (System.Drawing.Point e )
		{
			// Get the size of the display window
			int nDisplayHeight = axuEyeCam.Height;
			int nDisplayWidth = axuEyeCam.Width;
			int nImageHeight = 0;
			int nImageWidth = 0;

			// Get the image size
			axuEyeCam.GetImageSize(ref nImageWidth, ref nImageHeight);

			double dblScaleFactorX = 1.0;
			double dblScaleFactorY = 1.0;

			// Calculate the scaling factors between the image and the display 
			// to show a cross with the correct size
			if ((axuEyeCam.GetRenderMode() & uEye_defines.IS_RENDER_FIT_TO_WINDOW) != 0)
			{
				dblScaleFactorX = (double)nImageWidth / (double)nDisplayWidth;
				dblScaleFactorY = (double)nImageHeight / (double)nDisplayHeight;
			}

			// Create a pen with the color red
			System.Drawing.SolidBrush brush;
			brush = new System.Drawing.SolidBrush (System.Drawing.Color.Red);
			System.Drawing.Pen pen = new System.Drawing.Pen( brush, 3) ;

			// Calculate the correct position of the cross (depending on the scaling factors)
			int x = (int)((double)e.X * dblScaleFactorX);
			int y = (int)((double)e.Y * dblScaleFactorY);

			// Calculate the correct width and height of the cross
			int nCrossWidth = (int)(10 * dblScaleFactorX);
			int nCrossHeight = (int)(10 * dblScaleFactorY);
			
			System.Drawing.Point pt1 = new System.Drawing.Point( x - nCrossWidth, y - nCrossHeight );
			System.Drawing.Point pt2 = new System.Drawing.Point( x + nCrossWidth, y + nCrossHeight );
			System.Drawing.Point pt3 = new System.Drawing.Point( x + nCrossWidth, y - nCrossHeight );
			System.Drawing.Point pt4 = new System.Drawing.Point( x - nCrossWidth, y + nCrossHeight );
			
			// Set the Direct3D key color to black
			axuEyeCam.Direct3DKeyColor = System.Drawing.Color.Black;
			
			// Clear the Direct3D overlay
			axuEyeCam.ClearDirect3DOverlay ();

			// Get the DC of the Direct3D overlay
			int hdc = axuEyeCam.GetDC();
			System.Drawing.Graphics DC = System.Drawing.Graphics.FromHdc( (System.IntPtr)hdc );
			
			DC.DrawLine( pen, pt1, pt2 );
			DC.DrawLine( pen, pt3, pt4 );
			
			// Release the DC of the Direct3D overlay
			axuEyeCam.ReleaseDC( hdc );
		}

		private void TriggerOff_Click(object sender, System.EventArgs e)
		{
			if(m_bLive)
			{
				axuEyeCam.StopLiveVideo( uEye_defines.IS_WAIT );
			}

			// Disable trigger
            if (axuEyeCam.SetExternalTrigger(uEye_defines.IS_SET_TRIGGER_OFF) != uEye_defines.IS_SUCCESS)
            {
                if (m_nCurrentMode == 1)
                {
                    TriggerOff.Checked = false;
                    TriggerSoftware.Checked = true;
                    TriggerRising.Checked = false;
                    TriggerFalling.Checked = false;
                }
                else if (m_nCurrentMode == 2)
                {
                    TriggerOff.Checked = false;
                    TriggerSoftware.Checked = false;
                    TriggerRising.Checked = false;
                    TriggerFalling.Checked = true;
                }
                else if (m_nCurrentMode == 3)
                {
                    TriggerOff.Checked = false;
                    TriggerSoftware.Checked = false;
                    TriggerRising.Checked = true;
                    TriggerFalling.Checked = false;
                }

                MessageBox.Show("The trigger mode can not be disabled with the current settings");
             
                                
                return;
            }

			if(m_bLive)
			{
				axuEyeCam.StartLiveVideo(1);
			}

            m_nCurrentMode = 0;
		}

		private void TriggerSoftware_Click(object sender, System.EventArgs e)
		{
			if(m_bLive)
			{
				axuEyeCam.StopLiveVideo( uEye_defines.IS_WAIT );
			}

			// Enable software trigger
			axuEyeCam.SetExternalTrigger( uEye_defines.IS_SET_TRIGGER_SOFTWARE );

			if(m_bLive)
			{
                if (m_bContinuousTriggerSupported)
                {
                    axuEyeCam.StartLiveVideo(1);
                }
                else
                {
                    // Stop capture
                    axuEyeCam.StopLiveVideo(uEye_defines.IS_WAIT);
                    m_bLive = false;
                    CaptureImage.Text = "Start capture";
                }
			}

            m_nCurrentMode = 1;
		}

		private void TriggerFalling_Click(object sender, System.EventArgs e)
		{
			if(m_bLive)
			{
				axuEyeCam.StopLiveVideo( uEye_defines.IS_WAIT );
			}

			// Enable hardware trigger falling
			axuEyeCam.SetExternalTrigger( uEye_defines.IS_SET_TRIGGER_HI_LO );

			if(m_bLive)
			{
				axuEyeCam.StartLiveVideoWait(100);
			}

            m_nCurrentMode = 2;
		}

		private void TriggerRising_Click(object sender, System.EventArgs e)
		{
			if(m_bLive)
			{
				axuEyeCam.StopLiveVideo( uEye_defines.IS_WAIT );
			}

			// Enable hardware trigger rising
			axuEyeCam.SetExternalTrigger( uEye_defines.IS_SET_TRIGGER_LO_HI );

			if(m_bLive)
			{
				axuEyeCam.StartLiveVideoWait(100);
			}

            m_nCurrentMode = 3;
		}
	}
}
