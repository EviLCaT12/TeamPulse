using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Performances.Contract.Dtos;
using TeamPulse.Performances.Domain.Entities;

namespace TeamPulse.Performances.Infrastructure.Configurations.Read;

public class RecordSkillDtoConfiguration : IEntityTypeConfiguration<RecordSkillDto>
{
    public void Configure(EntityTypeBuilder<RecordSkillDto> builder)
    {
        builder.ToTable("record_skills");
        
        builder.HasKey(x => new {x.WhoId, x.WhatId});
    }
}