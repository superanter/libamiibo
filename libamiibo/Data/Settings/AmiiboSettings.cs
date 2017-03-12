﻿/*
 * Copyright (C) 2016 Benjamin Krämer
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibAmiibo.Data.Settings.AppData;
using LibAmiibo.Data.Settings.UserData;
using LibAmiibo.Helper;

namespace LibAmiibo.Data.Settings
{
    public class AmiiboSettings
    {
        public ArraySegment<byte> CryptoBuffer { get; private set; }
        public AmiiboUserData AmiiboUserData { get; private set; }
        public AmiiboAppData AmiiboAppData { get; private set; }

        private IList<byte> CryptoBufferList
        {
            get { return CryptoBuffer as IList<byte>; }
        }

        public Status Status
        {
            get { return (Status) (CryptoBufferList[0] & 0x30); }
        }

        public ushort AmiiboLastModifiedDateValue
        {
            get { return NtagHelpers.UInt16FromTag(CryptoBuffer, 0x06); }
        }

        public DateTime AmiiboLastModifiedDate
        {
            get
            {
                return NtagHelpers.DateTimeFromTag(AmiiboLastModifiedDateValue);
            }
        }

        public ushort WriteCounter
        {
            get { return NtagHelpers.UInt16FromTag(CryptoBuffer, 0x88); }
        }

        public ArraySegment<byte> Unknown8EBytes
        {
            get
            {
                return new ArraySegment<byte>(CryptoBuffer.Array, CryptoBuffer.Offset + 0x8E, 0x02);
            }
        }
        public ArraySegment<byte> Signature
        {
            get
            {
                return new ArraySegment<byte>(CryptoBuffer.Array, CryptoBuffer.Offset + 0x90, 0x20);
            }
        }

        public AmiiboSettings(ArraySegment<byte> cryptoData, ArraySegment<byte> appData)
        {
            this.CryptoBuffer = cryptoData;
            this.AmiiboUserData = new AmiiboUserData(cryptoData);
            this.AmiiboAppData = new AmiiboAppData(cryptoData, appData);
        }
    }
}
