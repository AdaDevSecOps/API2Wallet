using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.Coupon
{
    
    public class cmlResCouponList 
    {
        private int cValue;
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        /// รหัสเอกสาร
        /// </summary>
        public string rtCphDocNo { get; set; }

        /// <summary>
        /// ประเภทส่วนลด 1: ลดบาท , 2: ลด %
        /// </summary>
        public string rtCphDisType { get; set; }

        /// <summary>
        /// มูลค่าคูปอง
        /// </summary>
        public Nullable<decimal> rcCphDisValue { get; set; }
        //public int rcCphDisValue
        //{
        //    get { return cValue; }
        //    set { this.cValue = Convert.ToInt32(value); }
        //}
        /// <summary>
        /// ชื่อคูปอง
        /// </summary>
        public string rtCpnName { get; set; }

        /// <summary>
        /// ข้อความแสดงบนคูปองบันทัดที่ 1
        /// </summary>
        public string rtCpnMsg1 { get; set; }

        /// <summary>
        /// ข้อความแสดงบนคูปองบันทัดที่ 2
        /// </summary>
        public string rtCpnMsg2 { get; set; }

        /// <summary>
        /// เงื่อนไขแสดงบนคูปอง
        /// </summary>
        public string rtCpnCond { get; set; }

        /// <summary>
        /// คูปองบาร์ (หมายเลขคูปอง)
        /// </summary>
        public string rtCpdBarCpn { get; set; }

        /// <summary>
        /// (AUTONUMBER)ลำดับ
        /// </summary>
        public Nullable<Int64> rnCpdSeqNo { get; set; }

        /// <summary>
        /// ชื่อประเภท
        /// </summary>
        public string rtCptName { get; set; }

        /// <summary>
        /// ประเภท 1:คูปองเงินสด 2:คูปองส่วนลด
        /// </summary>
        public string rtCptType { get; set; }

        /// <summary>
        /// จำนวคงเหลือที่ใช้ได้
        /// </summary>
        public Nullable<Int64> rnQtyAvailable { get; set; }

        /// <summary>
        /// จำนวคงเหลือ
        /// </summary>
        public Nullable<Int64> rnQtyLef { get; set; }

        /// <summary>
        /// วันที่เริ่มใช้งาน
        /// </summary>
        public Nullable<DateTime> rdCphDateStart { get; set; }  //*Arm 63-01-08 

        /// <summary>
        /// วันสิ้นสุดใช้งาน
        /// </summary>
        public Nullable<DateTime> rdCphDateStop { get; set; }   //*Arm 63-01-08 

        /// <summary>
        /// เวลาเริ่มใช้งาน
        /// </summary>
        public string rtCphTimeStart { get; set; }  //*Arm 63-01-08 

        /// <summary>
        /// เวลาสิ้นสุดใช้งาน
        /// </summary>
        public string rtCphTimeStop { get; set; }   //*Arm 63-01-08 

    }
}