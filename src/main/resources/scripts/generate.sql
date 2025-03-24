DELIMITER //
CREATE PROCEDURE GenerateBudgets(IN num INT)
BEGIN
    DECLARE i INT DEFAULT 0;
    DECLARE random_customer_id INT;

    WHILE i < num DO
            SELECT customer_id INTO random_customer_id
            FROM customer
            ORDER BY RAND()
            LIMIT 1;

            INSERT INTO budget (description, amount, customer_id, created_at)
            VALUES (
                       CONCAT('Expense for Customer ', random_customer_id),
                       RAND() * 100000 + 1000000,
                       random_customer_id,
                       DATE_SUB(NOW(), INTERVAL FLOOR(RAND() * 365) DAY)
                   );

            SET i = i + 1;
        END WHILE;
END //
DELIMITER ;

CALL GenerateBudgets(100);

DELIMITER //
CREATE PROCEDURE GenerateTickets(IN num INT)
BEGIN
    DECLARE i INT DEFAULT 0;
    DECLARE new_expense_id INT;
    DECLARE random_customer_id INT;

    WHILE i < num DO
            SELECT customer_id INTO random_customer_id
            FROM customer
            ORDER BY RAND()
            LIMIT 1;

            INSERT INTO expense (customer_id, budget_id, description, amount, created_at)
            VALUES (
                    random_customer_id,
                       RAND() * 500 + 50,  -- Random amount between 50 and 550
                       CONCAT('Expense for Ticket ', FLOOR(RAND() * 10000)),
                       TIMESTAMP(DATE_SUB(NOW(), INTERVAL FLOOR(RAND() * 365) DAY)
                           - INTERVAL FLOOR(RAND() * 24) HOUR
                           - INTERVAL FLOOR(RAND() * 60) MINUTE)
                   );

            -- Get the last inserted expense_id
            SET new_expense_id = LAST_INSERT_ID();

            -- Insert a ticket linked to the new expense and the selected customer
            INSERT INTO tickets (customer_id, issue, created_at, status, expense_id)
            VALUES (
                       random_customer_id,  -- Random existing customer ID
                       CONCAT('Issue ', FLOOR(RAND() * 10000)),
                       TIMESTAMP(DATE_SUB(NOW(), INTERVAL FLOOR(RAND() * 365) DAY)
                           - INTERVAL FLOOR(RAND() * 24) HOUR
                           - INTERVAL FLOOR(RAND() * 60) MINUTE),
                       CASE FLOOR(RAND() * 3)  -- 3 possible statuses
                           WHEN 0 THEN 'Open'
                           WHEN 1 THEN 'Closed'
                           WHEN 2 THEN 'Pending'
                           END,
                       new_expense_id
                   );

            SET i = i + 1;
        END WHILE;
END //
DELIMITER ;
