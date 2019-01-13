﻿using System;
using System.Collections.Generic;

namespace Aviadispetcher
{
    public class Flight
    {
        public Flight(string nF, string cF, System.TimeSpan tF, int fS)
        {
            this.number = nF;
            this.city = cF;
            this.depature_time = tF;
            this.free_seats = fS;
        }
        public string number { get; set; }
        public string city { get; set; }
        public System.TimeSpan depature_time { get; set; }
        public int free_seats { get; set; }
    }
}