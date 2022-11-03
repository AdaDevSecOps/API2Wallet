using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models
{
    /// <summary>
    /// Information class TFNMCard,TFNMCard_L
    /// </summary>
    public class cmlTFNMCard
    {
        /// <summary>
        /// รหัส [รูป TCNMImgPerson]
        /// </summary>
        public string FTCrdCode { get; set; }

        /// <summary>
        /// ชื่อ, ชื่อผู้ถือบัตร
        /// </summary>
        public string FTCrdName { get; set; }

        /// <summary>
        /// วันที่เริ่มใช้บัตรได้
        /// </summary>
        public DateTime FDCrdStartDate { get; set; }

        /// <summary>
        /// วันที่บัตรหมดอายุ
        /// </summary>
        public Nullable<DateTime> FDCrdExpireDate { get; set; }

        /// <summary>
        /// รหัสประเภทบัตร
        /// </summary>
        public string FTCtyCode { get; set; }

        /// <summary>
        /// ยอดทั้งหมด
        /// </summary>
        public decimal FCCrdValue { get; set; }

        /// <summary>
        /// วันที่เติมเงินล่าสุด
        /// </summary>
        public Nullable<DateTime> FDCrdLastTopup  { get; set; }
       
        /// <summary>
        /// ยอดมัดจำบัตร
        /// </summary>
        public decimal FCCrdDeposit { get; set; }

        /// <summary>
        /// ยอดมัดจำสินค้า
        /// </summary>
        public decimal FCCrdDepositPdt { get; set; }

        /// <summary>
        /// ยอดใช้ได้
        /// </summary>
        public decimal cAvailable { get; set; }

        /// <summary>
        /// ชื่อ
        /// </summary>
        public string FTCtyName { get; set; }

        /// <summary>
        /// รหัสภาษา
        /// </summary>
        public Int64 FNLngID { get; set; }

        /// <summary>
        /// จำนวน Transaction ที่ ประมวลผลตัดยอดแล้ว  (ได้จาก การประมวลผลตัดยอดที่ละรายการ)
        /// </summary>
        public Nullable<Int64> FNCrdTxnPrcAdj { get; set; }

        /// <summary>
        /// รหัสพนักงาน,หมายเลขผู้ถือบัตร
        /// </summary>
        public string FTCrdHolderID { get; set; }
        public string FTCrdRefID { get; set; }
        public string FTCrdStaType { get; set; }
        //public string FTCrdStaLocate { get; set; }
        public string FTCrdStaActive { get; set; }

        /// <summary>
        /// จำนวนอายุ บัตร
        /// </summary>
        public Nullable<Int64> FNCtyExpirePeriod { get; set; }

        /// <summary>
        /// ประเภทหมดอายุบัตร
        /// </summary>
        public Nullable<int> FNCtyExpiredType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FTCtyStaAlwRet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal FCCtyTopupAuto { get; set; }

        /// <summary>
        /// สถานะเบิกบัตร 1:ยังไม่ได้เบิก 2:เบิกแล้ว
        /// </summary>
        public string FTCrdStaShift { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FTCrdStaLocate { get; set; }
    }
}