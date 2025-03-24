create or replace  view v_lead_expense as (
select amount ,l.created_at from trigger_lead l join expense e on e.expense_id = l.expense_id );

create or replace  view v_ticket_expense as (
select amount ,l.created_at from trigger_lead l join expense e on e.expense_id = l.expense_id );