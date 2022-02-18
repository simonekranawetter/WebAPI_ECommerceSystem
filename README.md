# WebAPI_ECommerceSystem
Final project for the Web API course with ECUtbildning


# Basic Requirements

## CRUD a user

- First and Last Name
- Email
- Password
- Address-information
- Other contact information that might be relevant
- When a user is removed ORDERS must stay the same, but user info should be removed safely without breaking everything

## CRUD a product

- Article number
- Product Name
- Description
- Price
- Category

## CRUD an order

- Who has ordered
- Order date
- Order status
- Product(s) ordered
- Number of Products ordered

The API should be secured with the help of a key so only users with the key can use the API

# To get the better grade

- Secure the API so orders can only be placed by eligible users
- A user must be stored with a secure password that is encrypted in some way
- To use the API the user has to log in with his email and password
- Only administrators should be able to do certain things such as create products, update products etc. This should be done with some form of key

## Features implemented that were not required

- Verification with data annotations on DTOs
Due to time constraints mapping has not been implemented with leads to some repeat code.
