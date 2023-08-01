﻿namespace APICatalog.DTOs
{
    public abstract class OutputDTOBase
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}
