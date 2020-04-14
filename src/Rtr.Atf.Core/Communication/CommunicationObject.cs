using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// Represents log data received from device.
    /// </summary>
    public partial class CommunicationObject
    {
        /// <summary>
        /// Gets or sets a collection of device communication transactions.
        /// </summary>
        [JsonProperty("lines")]
        public List<Transaction> Transactions { get; set; }
    }

#pragma warning disable SA1402 // File may only contain a single type
    /// <summary>
    /// Represents a single device communication transaction.
    /// </summary>
    public partial class Transaction
#pragma warning restore SA1402 // File may only contain a single type
    {
        /*
         * All underlying properties are not covered with comments,
         * since they reflect 3rd party's software data representation
         * and, in theory, are subjects to change.
        */

        [JsonProperty("Addr")]
#pragma warning disable SA1600 // Elements should be documented
        public long Addr { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("AddrType")]
#pragma warning disable SA1600 // Elements should be documented
        public long AddrType { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("BurstMode")]
#pragma warning disable SA1600 // Elements should be documented
        public long BurstMode { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("ByteCnt")]
#pragma warning disable SA1600 // Elements should be documented
        public long ByteCnt { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("CRC")]
#pragma warning disable SA1600 // Elements should be documented
        public long Crc { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("Channel")]
#pragma warning disable SA1600 // Elements should be documented
        public string Channel { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("Cmd")]
#pragma warning disable SA1600 // Elements should be documented
        public long Cmd { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("CmdName")]
#pragma warning disable SA1600 // Elements should be documented
        public string CmdName { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("Data")]
#pragma warning disable SA1600 // Elements should be documented
        public List<long> Data { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("Delimiter")]
#pragma warning disable SA1600 // Elements should be documented
        public long Delimiter { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("DevId")]
#pragma warning disable SA1600 // Elements should be documented
        public long DevId { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("DevSts")]
#pragma warning disable SA1600 // Elements should be documented
        public long DevSts { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("DevType")]
#pragma warning disable SA1600 // Elements should be documented
        public long DevType { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("ExpBytes")]
#pragma warning disable SA1600 // Elements should be documented
        public List<object> ExpBytes { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("FrameType")]
#pragma warning disable SA1600 // Elements should be documented
        public long FrameType { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("GenFrameType")]
#pragma warning disable SA1600 // Elements should be documented
        public long GenFrameType { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("HostFrameType")]
#pragma warning disable SA1600 // Elements should be documented
        public string HostFrameType { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("HostSts")]
#pragma warning disable SA1600 // Elements should be documented
        public string HostSts { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("LineProperties")]
#pragma warning disable SA1600 // Elements should be documented
        public long LineProperties { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("LogLine")]
#pragma warning disable SA1600 // Elements should be documented
        public string LogLine { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("LogLineOdds")]
#pragma warning disable SA1600 // Elements should be documented
        public List<object> LogLineOdds { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("MasterAddr")]
#pragma warning disable SA1600 // Elements should be documented
        public long MasterAddr { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("MsgID")]
#pragma warning disable SA1600 // Elements should be documented
        public string MsgId { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("NumExpBytes")]
#pragma warning disable SA1600 // Elements should be documented
        public long NumExpBytes { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("PhyLayerType")]
#pragma warning disable SA1600 // Elements should be documented
        public long PhyLayerType { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("Pocket")]
#pragma warning disable SA1600 // Elements should be documented
        public Pocket Pocket { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("RC")]
#pragma warning disable SA1600 // Elements should be documented
        public long Rc { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("Time")]
#pragma warning disable SA1600 // Elements should be documented
        public DateTimeOffset Time { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("Timestamp")]
#pragma warning disable SA1600 // Elements should be documented
        public double Timestamp { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("mapo")]
#pragma warning disable SA1600 // Elements should be documented
        public Mapo Mapo { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("rego")]
#pragma warning disable SA1600 // Elements should be documented
        public Rego Rego { get; set; }
#pragma warning restore SA1600 // Elements should be documented

#pragma warning disable SA1600 // Elements should be documented
        public override string ToString() => $"{this.Cmd}, {this.CmdName}, {this.GenFrameType}";
#pragma warning restore SA1600 // Elements should be documented
    }

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1601 // Partial elements should be documented
    public partial class Mapo
#pragma warning restore SA1601 // Partial elements should be documented
#pragma warning restore SA1402 // File may only contain a single type
    {
        [JsonProperty("index_not_found")]
#pragma warning disable SA1600 // Elements should be documented
        public bool IndexNotFound { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("name")]
#pragma warning disable SA1600 // Elements should be documented
        public string Name { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("out_of_range")]
#pragma warning disable SA1600 // Elements should be documented
        public bool OutOfRange { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("param_dict")]
#pragma warning disable SA1600 // Elements should be documented
        public List<ParamDict> ParamDict { get; set; }
#pragma warning restore SA1600 // Elements should be documented

#pragma warning disable SA1600 // Elements should be documented
        public override string ToString() => $"{this.Name}, params count: {this.ParamDict?.Count}";
#pragma warning restore SA1600 // Elements should be documented
    }

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1601 // Partial elements should be documented
    public partial class ParamDict
#pragma warning restore SA1601 // Partial elements should be documented
#pragma warning restore SA1402 // File may only contain a single type
    {
        [JsonProperty("desc")]
#pragma warning disable SA1600 // Elements should be documented
        public string Desc { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("index")]
#pragma warning disable SA1600 // Elements should be documented
        public long Index { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("val_num")]
#pragma warning disable SA1600 // Elements should be documented
        public string ValNum { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("val_str")]
#pragma warning disable SA1600 // Elements should be documented
        public string ValStr { get; set; }
#pragma warning restore SA1600 // Elements should be documented

#pragma warning disable SA1600 // Elements should be documented
        public override string ToString() => $"{this.Index}, {this.Desc}, {this.ValStr}";
#pragma warning restore SA1600 // Elements should be documented
    }

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1601 // Partial elements should be documented
    public partial class Pocket
#pragma warning restore SA1601 // Partial elements should be documented
#pragma warning restore SA1402 // File may only contain a single type
    {
    }

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1601 // Partial elements should be documented
    public partial class Rego
#pragma warning restore SA1601 // Partial elements should be documented
#pragma warning restore SA1402 // File may only contain a single type
    {
        [JsonProperty("addr")]
#pragma warning disable SA1600 // Elements should be documented
        public long Addr { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("cmd")]
#pragma warning disable SA1600 // Elements should be documented
        public long Cmd { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("cmd_str_suffix")]
#pragma warning disable SA1600 // Elements should be documented
        public string CmdStrSuffix { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("cmd_width")]
#pragma warning disable SA1600 // Elements should be documented
        public long CmdWidth { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("dev_type")]
#pragma warning disable SA1600 // Elements should be documented
        public long DevType { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("gen_frame_type")]
#pragma warning disable SA1600 // Elements should be documented
        public long GenFrameType { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("hreg_cmd")]
#pragma warning disable SA1600 // Elements should be documented
        public long HregCmd { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("hreg_cmd_args")]
#pragma warning disable SA1600 // Elements should be documented
        public List<object> HregCmdArgs { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("is_65k")]
#pragma warning disable SA1600 // Elements should be documented
        public bool Is65K { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("is_consecutive")]
#pragma warning disable SA1600 // Elements should be documented
        public bool IsConsecutive { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("nc_reg_data")]
#pragma warning disable SA1600 // Elements should be documented
        public List<List<long>> NcRegData { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("nc_reg_num")]
#pragma warning disable SA1600 // Elements should be documented
        public long NcRegNum { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("nc_regs")]
#pragma warning disable SA1600 // Elements should be documented
        public List<long> NcRegs { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("params")]
#pragma warning disable SA1600 // Elements should be documented
        public List<object> Params { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("proto")]
#pragma warning disable SA1600 // Elements should be documented
        public long Proto { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("reg_cat")]
#pragma warning disable SA1600 // Elements should be documented
        public string RegCat { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("reg_data")]
#pragma warning disable SA1600 // Elements should be documented
        public List<object> RegData { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("reg_def_db")]
#pragma warning disable SA1600 // Elements should be documented
        public Pocket RegDefDb { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("reg_num")]
#pragma warning disable SA1600 // Elements should be documented
        public long RegNum { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("reg_start")]
#pragma warning disable SA1600 // Elements should be documented
        public long RegStart { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("reg_stop")]
#pragma warning disable SA1600 // Elements should be documented
        public long RegStop { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("regs")]
#pragma warning disable SA1600 // Elements should be documented
        public List<Reg> Regs { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("write")]
#pragma warning disable SA1600 // Elements should be documented
        public bool Write { get; set; }
#pragma warning restore SA1600 // Elements should be documented

#pragma warning disable SA1600 // Elements should be documented
        public override string ToString()
#pragma warning restore SA1600 // Elements should be documented
        {
            if (this.Regs == null || !this.Regs.Any())
            {
                return string.Empty;
            }

            var regNums = this.Regs.Select(r => r.RegNum).OrderBy(l => l);

            return string.Join(", ", regNums);
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1601 // Partial elements should be documented
    public partial class Reg
#pragma warning restore SA1601 // Partial elements should be documented
#pragma warning restore SA1402 // File may only contain a single type
    {
        [JsonProperty("def")]
#pragma warning disable SA1600 // Elements should be documented
        public string Def { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("lname")]
#pragma warning disable SA1600 // Elements should be documented
        public string Lname { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("reg_num")]
#pragma warning disable SA1600 // Elements should be documented
        public long RegNum { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("type")]
#pragma warning disable SA1600 // Elements should be documented
        public string Type { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("val_num")]
#pragma warning disable SA1600 // Elements should be documented
        public string ValNum { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("val_num_bits")]
#pragma warning disable SA1600 // Elements should be documented
        public List<ValNumBit> ValNumBits { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("val_str")]
#pragma warning disable SA1600 // Elements should be documented
        public string ValStr { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("width")]
#pragma warning disable SA1600 // Elements should be documented
        public long Width { get; set; }
#pragma warning restore SA1600 // Elements should be documented

#pragma warning disable SA1600 // Elements should be documented
        public override string ToString() => $"{this.Lname}, {this.RegNum}, {this.Type}, {this.Def}, (Num - {this.ValNum}, Str - {this.ValStr}) ";
#pragma warning restore SA1600 // Elements should be documented
    }

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1601 // Partial elements should be documented
    public partial class ValNumBit
#pragma warning restore SA1601 // Partial elements should be documented
#pragma warning restore SA1402 // File may only contain a single type
    {
        [JsonProperty("index")]
#pragma warning disable SA1600 // Elements should be documented
        public long Index { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("name")]
#pragma warning disable SA1600 // Elements should be documented
        public string Name { get; set; }
#pragma warning restore SA1600 // Elements should be documented

        [JsonProperty("set")]
#pragma warning disable SA1600 // Elements should be documented
        public bool Set { get; set; }
#pragma warning restore SA1600 // Elements should be documented

#pragma warning disable SA1600 // Elements should be documented
        public override string ToString() => $"{this.Index}, {this.Name}, {this.Set}";
#pragma warning restore SA1600 // Elements should be documented
    }
}
