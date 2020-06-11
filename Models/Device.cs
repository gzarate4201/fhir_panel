using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{
    
    // Definicion del Objeto Devices (tabla para almacenar la informacion de configuracion de los dispositivos)
    [Table("devices")]
    public class Device
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("device_id")]
        public string DevId { get; set; }

        [Column("device_tag")]
        public string DevTag { get; set; }

        [Column("device_token")]
        public string DevTkn { get; set; }

        [Column("dev_pwd")]
        public string DevPwd { get; set; }

        [Column("dev_name")]
        public string DevName { get; set; }

        [Column("ip_addr")]
        public string IpAddr { get; set; }

        [Column("net_mask")]
        public string NetMsk { get; set; }

        [Column("gateway")]
        public string NetGw { get; set; }

        [Column("DDNS1")]
        public string DDNS1 { get; set; }

        [Column("DDNS2")]
        public string DDNS2 { get; set; }

        [Column("DHCP")]
        public string DHCP { get; set; }

        [Column("dec_face_num_cur")]
        public string DecFaceNumCur { get; set; }

        [Column("dec_interval_cur")]
        public string DecIntCur { get; set; }

        [Column("dec_face_num_min")]
        public string DecFaceNumMin { get; set; }

        [Column("dec_face_num_max")]
        public string DecFaceNumMax { get; set; }

        [Column("dec_interval_min")]
        public string DecIntMin { get; set; }

        [Column("dec_interval_max")]
        public string DecIntMax { get; set; }

        [Column("dev_model")]
        public string DevMdl { get; set; }

        [Column("firmware_ver")]
        public string FwrVer { get; set; }

        [Column("firmware_date")]
        public string FwrDate { get; set; }

        [Column("temp_dec_en")]
        public string TempDecEn { get; set; }

        [Column("stranger_pass_en")]
        public string StrPassEn { get; set; }

        [Column("make_check_en")]
        public string MkeChkEn { get; set; }

        [Column("alarm_temp")]
        public string AlarmTemp { get; set; }

        [Column("temp_comp")]
        public string TempComp { get; set; }

        [Column("record_time_save")]
        public string RcrdTimeSv { get; set; }

        [Column("save_record")]
        public string SvRec { get; set; }

        [Column("save_jpg")]
        public string SvJpg { get; set; }

        [Column("mqtt_enable")]
        public string MqttEn { get; set; }

        [Column("mqtt_retain")]
        public string MqttRet { get; set; }

        [Column("pqos")]
        public string PQos { get; set; }

        [Column("sqos")]
        public string SQos { get; set; }

        [Column("maqtt_port")]
        public string MqttPrt { get; set; }

        [Column("mqtt_server")]
        public string MqttSrv { get; set; }

        [Column("mqtt_username")]
        public string MqttUsr { get; set; }

        [Column("maqtt_password")]
        public string MqttPwd { get; set; }

        [Column("topic2publish")]
        public string Topic2Pub { get; set; }

        [Column("topic2subscribe")]
        public string Topic2Sub { get; set; }

        [Column("heartbeat")]
        public string HeartBt { get; set; }
    }
}
