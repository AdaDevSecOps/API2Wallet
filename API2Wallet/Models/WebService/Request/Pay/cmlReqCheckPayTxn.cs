using API2Wallet.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Request.Pay
{
    /// <summary>
    /// Check payment transaction parameter.
    /// </summary>
    public class cmlReqCheckPayTxn
    {
        /// <summary>
        /// Document no.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptDocNo { get; set; }

        /// <summary>
        /// POS code.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(3, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPosCode { get; set; }

        /// <summary>
        /// Branch code.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptBchCode { get; set; }

        /// <summary>
        /// Document type.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptDocType { get; set; }

        /// <summary>
        /// Language ID.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public Nullable<int> pnLngID { get; set; }

        /// <summary>
        /// System date from client.
        /// </summary>
        /// <remarks>
        /// *[AnUBiS][][2019-06-14]
        /// </remarks>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public Nullable<DateTime> pdSysDate { get; set; }

        /// <summary>
        /// Condition system date previous.
        /// </summary>
        /// <remarks>
        /// *[AnUBiS][][2019-06-14]
        /// </remarks>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public Nullable<int> pnPrevious { get; set; }

    }
}