-- FUNCTION: public.sum_avg()

-- DROP FUNCTION IF EXISTS public.sum_avg();

CREATE OR REPLACE FUNCTION public.sum_avg(
	OUT sum_val numeric,
	OUT avg_val numeric)
    RETURNS record
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
begin
  
  select SUM("IntNumber"),
		 AVG("DoubleNumber")
  into sum_val, avg_val
  from public."FileLines";
end;
$BODY$;

ALTER FUNCTION public.sum_avg()
    OWNER TO postgres;
