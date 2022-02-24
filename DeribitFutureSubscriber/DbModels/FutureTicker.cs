using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeribitFutureSubscriber.DbModels
{
    public class FutureTicker
    {
        [Key]
        [Column("id")]
        public int Id { get; set; } 

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("settlement_price")]
        public decimal SettlementPrice { get; set; }

        [Column("open_interest")]
        public decimal OpenInterest { get; set; }
    }
}

/*
CREATE TABLE public."FutureTickers"
(
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( START 0 MINVALUE 0 ),
    name character varying[],
    open_interest numeric,
    settlement_price numeric,
    PRIMARY KEY (id)
);

ALTER TABLE IF EXISTS public."FutureTickers"
    OWNER to postgres;
*/
