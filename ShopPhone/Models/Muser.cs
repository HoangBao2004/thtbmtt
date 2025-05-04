namespace ShopNoiThat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("user")]
    public partial class Muser
    {
        public int ID { get; set; }

        [StringLength(255)]
        public string fullname { get; set; }

        [StringLength(225)]
        public string username { get; set; }

        [StringLength(64)]
        public string password { get; set; }

        [StringLength(255)]
        public string email { get; set; }

        [StringLength(5)]
        public string gender { get; set; }

        [StringLength(20)]
        public string phone { get; set; }

        [StringLength(100)]
        public string img { get; set; }

        public int access { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime created_at { get; set; }

        public int created_by { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime updated_at { get; set; }

        public int updated_by { get; set; }

        public int status { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? locked_until { get; set; }  // Thêm thuộc tính khóa tài khoản

        // Các thuộc tính thêm vào:
        public int? failed_login_count { get; set; }  // Số lần đăng nhập thất bại

        [Column(TypeName = "smalldatetime")]
        public DateTime? last_failed_login { get; set; }  // Thời gian đăng nhập thất bại cuối cùng

        [StringLength(6)]
        public string otp_code { get; set; }  // Mã OTP
        [Column(TypeName = "smalldatetime")]
        public DateTime? otp_created_at { get; set; }  // Thời gian tạo mã OTP
    }
}
