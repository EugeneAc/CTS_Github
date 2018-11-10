using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataEmulator
{
  public partial class CTS_Emulator : Form
  {
    System.Windows.Forms.Timer MyTimer = new System.Windows.Forms.Timer();
    Random rnd = new Random();
    private static bool emulation = false;

    public CTS_Emulator()
    {
      InitializeComponent();
      MyTimer.Tick += new EventHandler(MyTimer_Tick);
    }

    private void WagonTransfers_CheckedChanged(object sender, EventArgs e)
    {
      if (WagonTransfers.Checked)
      {
        wqntfrom.Enabled = true;
        wqntto.Enabled = true;
        wbruttofrom.Enabled = true;
        wbruttoto.Enabled = true;
        wtarefrom.Enabled = true;
        wtareto.Enabled = true;
      }
      else
      {
        wqntfrom.Enabled = false;
        wqntto.Enabled = false;
        wbruttofrom.Enabled = false;
        wbruttoto.Enabled = false;
        wtarefrom.Enabled = false;
        wtareto.Enabled = false;
      }
    }

    private void VehiTransfers_CheckedChanged(object sender, EventArgs e)
    {
      if(VehiTransfers.Checked)
      {
        vbruttofrom.Enabled = true;
        vbruttoto.Enabled = true;
        vtarefrom.Enabled = true;
        vtareto.Enabled = true;
      }
      else
      {
        vbruttofrom.Enabled = false;
        vbruttoto.Enabled = false;
        vtarefrom.Enabled = false;
        vtareto.Enabled = false;
      }
    }

    private void SkipTransfers_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void BeltTransfers_CheckedChanged(object sender, EventArgs e)
    {
      if(BeltTransfers.Checked)
      {
        bqntfrom.Enabled = true;
        bqntto.Enabled = true;
      }
      else
      {
        bqntfrom.Enabled = false;
        bqntto.Enabled = false;
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      dataCheck();

      emulation = !emulation;
      if (emulation)
      {
        button1.Text = "Stop";
        label22.Text = "Эмуляция начата";
        timerStart();

      }        
      else
      {
        button1.Text = "Start";
        label22.Text = "Эмуляция остановлена";
        MyTimer.Stop();
      }
    }

    private void MyTimer_Tick(object sender, EventArgs e)
    {
      if (emulation)
      {
        MyTimer.Stop();
        dataCheck();

        if (WagonTransfers.Checked)
        {
          Emulator.wagonTransfersEmul(Decimal.ToInt32(wqntfrom.Value), Decimal.ToInt32(wqntto.Value), Decimal.ToInt32(wbruttofrom.Value),
            Decimal.ToInt32(wbruttoto.Value), Decimal.ToInt32(wtarefrom.Value), Decimal.ToInt32(wtareto.Value));
        }

        if(BeltTransfers.Checked)
          Emulator.beltTransfersEmul(Decimal.ToInt32(bqntfrom.Value), Decimal.ToInt32(bqntto.Value));

        if(SkipTransfers.Checked)
          Emulator.skipTransfersEmul();

        if(VehiTransfers.Checked)
          Emulator.vehiTransfersEmul(Decimal.ToInt32(vbruttofrom.Value), Decimal.ToInt32(vbruttoto.Value),
            Decimal.ToInt32(vtarefrom.Value), Decimal.ToInt32(vtareto.Value));

        Emulator.cycleQnt++;
        label26.Text = "Количество циклов: " + Emulator.cycleQnt;
        timerStart();
      }
    }

    private void timerStart()
    {
      int delayTime = rnd.Next(Decimal.ToInt32(tqntfrom.Value), Decimal.ToInt32(tqntto.Value));
      MyTimer.Interval = (delayTime * 1000); // in seconds
      MyTimer.Start();
    }

    private void dataCheck()
    {
      if (wqntto.Value < wqntfrom.Value)
        wqntto.Value = wqntfrom.Value;

      if (wbruttofrom.Value < wtareto.Value)
        wbruttofrom.Value = wtareto.Value + 1;

      if (wbruttoto.Value < wbruttofrom.Value)
        wbruttoto.Value = wbruttofrom.Value;

      if (wtareto.Value < wtarefrom.Value)
        wtareto.Value = wtarefrom.Value;

      if (wqntto.Value >= tqntfrom.Value)
        tqntfrom.Value = wqntto.Value + 1;


      if (vbruttofrom.Value < vtareto.Value)
        vbruttofrom.Value = vtareto.Value;

      if (vbruttoto.Value < vbruttofrom.Value)
        vbruttoto.Value = vbruttofrom.Value;

      if (vtareto.Value < vtarefrom.Value)
        vtareto.Value = vtarefrom.Value;

      if (bqntto.Value < bqntfrom.Value)
        bqntto.Value = bqntfrom.Value;

      if (tqntto.Value < tqntfrom.Value)
        tqntto.Value = tqntfrom.Value;
    }
  }
}
