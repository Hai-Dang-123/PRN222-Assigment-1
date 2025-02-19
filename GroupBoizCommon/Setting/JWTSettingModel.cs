﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBoizCommon.Setting
{
    public class JWTSettingModel
    {
        /// <summary>
        /// The Secret key of the jwt to generate access token.
        /// </summary>
        public static string SecretKey { get; set; } = "MgmI*//'tx\r\nv,9u8D7HBU\r\nq\"UB~w8:OX\r\nj4#bC:5#Ia\r\nP<3h\\fjy\\'\r\nUk5kWjKF&P\r\nF@!,4wz~)w\r\nemBA^\"`8)c\r\nTXRy5QLlU)\r\nS}q^pnr\"m";

        /// <summary>
        /// The expire days of the jwt to generate access token.
        /// </summary>
        public static int ExpireDayAccessToken { get; set; } = 15;

        /// <summary>
        /// The expire days of the jwt to generate refresh token.
        /// </summary>
        public static int ExpireDayRefreshToken { get; set; } = 7;

        /// <summary>
        /// The issuer of the token.
        /// </summary>
        public static string Issuer { get; set; } = "https://localhost:7155"; // Thay đổi cho phù hợp

        /// <summary>
        /// The audience of the token.
        /// </summary>
        public static string Audience { get; set; } = "https://localhost:3000"; // Thay đổi cho phù hợp
    }
}
