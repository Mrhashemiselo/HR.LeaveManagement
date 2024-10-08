﻿namespace HR.LeaveManagement.Domain.Common;
public class BaseDomainEntity
{
    public int Id { get; set; }
    public DateTime DateCreated { get; set; }
    public string CreatedBy { get; set; }
    public DateTime LastModifyDate { get; set; }
    public string LastModifiedBy { get; set; }
}
