﻿using API2Wallet.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.Topup
{
    public class cmlResaoTopupList : cmlResBese
    {
        public List<cmlResTopupList> roTopupList { get; set; }
    }
}