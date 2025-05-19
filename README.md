# SQL Injection Demo 🚀

## Overview
This project is designed to demonstrate the concept and risks of SQL Injection, a common web security vulnerability. It provides a simple application with intentionally insecure code that allows users to observe how SQL Injection attacks can be performed and the potential consequences. The goal is to educate developers and security enthusiasts about the importance of secure coding practices and how to prevent SQL Injection vulnerabilities in real-world applications.

## Attack Scenarios

### Attack 1: Data Extraction via Union-Based SQL Injection 🔍
- **Context:** The customer search endpoint (`/api/customers/search`) accepts a name parameter and builds SQL queries by concatenating user input.
- **Vulnerable Query:** `SELECT * FROM Customers WHERE name = '{userInput}'`
- **Attack Input:** `John' UNION SELECT NULL, CAST(cnp AS VARCHAR), CAST(wage AS VARCHAR), NULL FROM Employees --`
- **Explanation:** The injected UNION query selects sensitive data (cnp and wage) from the Employees table, casting them to VARCHAR to match the column types. NULL placeholders align the number of columns in the unioned query. The payload fits within the varchar(50) limit of the name column.
- **Resulting Query:** `SELECT * FROM Customers WHERE name = 'John' UNION SELECT NULL, CAST(cnp AS VARCHAR), CAST(wage AS VARCHAR), NULL FROM Employees --'`
- **Impact:** Sensitive employee information is exposed, which can lead to financial fraud, identity theft, or blackmail.

### Attack 2: Authentication Bypass 🔑
- **Context:** The login endpoint uses a vulnerable query to authenticate customers by phone number.
- **Vulnerable Query:** `SELECT * FROM Customers WHERE phone_number = '{userInput}' AND shop_id = 1`
- **Attack Input:** `1234567890' OR 1=1 --`
- **Explanation:** The injected condition OR 1=1 causes the query to return all rows. The -- comments out the shop ID filter, bypassing authentication checks.
- **Resulting Query:** `SELECT * FROM Customers WHERE phone_number = '1234567890' OR 1=1 --' AND shop_id = 1`
- **Impact:** The attacker is authenticated as the first customer in the database, gaining unauthorized access to their account and sensitive data.

### Attack 3: Database Schema Enumeration 📊
- **Context:** An attacker can retrieve database metadata through the customer search endpoint.
- **Vulnerable Query:** `SELECT * FROM Customers WHERE name = '{userInput}'`
- **Attack Input:** `John' AND 1=0 UNION SELECT NULL, TABLE_NAME, COLUMN_NAME, NULL FROM INFORMATION_SCHEMA.COLUMNS --`
- **Explanation:** The condition AND 1=0 ensures the original query returns no rows. The UNION query retrieves table and column names from INFORMATION_SCHEMA.COLUMNS. The structure fits the four-column format expected by the original query.
- **Resulting Query:** `SELECT * FROM Customers WHERE name = 'John' AND 1=0 UNION SELECT NULL, TABLE_NAME, COLUMN_NAME, NULL FROM INFORMATION_SCHEMA.COLUMNS --'`
- **Impact:** The attacker maps the database schema, enabling more targeted and damaging future attacks.

## Mitigation Recommendations 🛡️
- Use parameterized queries to safely handle all user inputs.
- Sanitize and validate inputs rigorously.
- Restrict database user permissions to prevent access to sensitive tables and metadata.
- Implement logging and monitoring to detect abnormal query patterns.
- Avoid detailed error messages that can aid attackers.

## Testing with Swagger 🔧
You can test the endpoints of this application using Swagger. Swagger provides an interactive API documentation that allows you to explore and test the available endpoints directly from your browser. To access Swagger, run the application and navigate to `/swagger` in your web browser.

## How to Clone This Project 📋
To clone this project, follow these steps:

1. Open your terminal or command prompt.
2. Run the following command:
   ```bash
   git clone https://github.com/yourusername/SQLInjectionDemo.git
   ```
3. Navigate into the project directory:
   ```bash
   cd SQLInjectionDemo
   ```
4. Open the project in your preferred IDE (e.g., Visual Studio, Visual Studio Code).
5. Build and run the application to see it in action! 