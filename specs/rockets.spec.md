# Rocket Management API Specification
## Problem Description
- As a travel operations manager, I want to **create rocket records with a name, mission range, and passenger capacity** so that available vehicles can be configured for AstroBookings trips.
- As a booking coordinator, I want to **view and maintain rocket records** so that trip planning always uses current rocket information.
- As a compliance officer, I want to **enforce valid rocket data constraints** so that unsafe or unsupported rocket configurations cannot be used.

## Solution Overview
- Add a Rocket Management API capability that supports creating, viewing, updating, and deleting rocket records.
- Store each rocket with three core attributes: name, range, and capacity.
- Apply business validation so only supported ranges (`suborbital`, `orbital`, `moon`, `mars`) and capacities (1 to 10 passengers) are accepted.
- Return clear success and error responses so client applications can reliably manage rocket data.

## Acceptance Criteria
- [ ] When a client submits a request to create a rocket with a valid name, range, and capacity, the system shall create the rocket and return the created rocket details.
- [ ] If a client submits a create or update request with a range outside `suborbital`, `orbital`, `moon`, or `mars`, then the system shall reject the request and return a validation error.
- [ ] If a client submits a create or update request with capacity lower than 1 or higher than 10, then the system shall reject the request and return a validation error.
- [ ] If a client submits a create or update request without a rocket name, then the system shall reject the request and return a validation error.
- [ ] When a client requests the rocket list, the system shall return all stored rockets with their name, range, and capacity.
- [ ] When a client requests a specific rocket that exists, the system shall return that rocket’s details.
- [ ] If a client requests a specific rocket that does not exist, then the system shall return a not-found response.
- [ ] When a client updates an existing rocket with valid values, the system shall persist the changes and return the updated rocket details.
- [ ] When a client deletes an existing rocket, the system shall remove it from storage and future retrieval requests for that rocket shall return not found.
