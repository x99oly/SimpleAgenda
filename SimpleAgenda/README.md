# 🗓️ SimpleAgenda – Project Overview

## ✅ Features Implemented

### 🧱 1. Schedule Entity
- Defines recurrence rules for Appointments.
- Built with immutable structs:
  - `DateRange`
  - `HourMinute`
  - `DaysOfWeekCollection`
  - `Recurrence`
- Validations:
  - `StartDate` ≥ `DateTime.UtcNow`
  - `EndDate` > `StartDate`
  - `RecurrenceInterval` ≥ 1
  - Valid hour and minute ranges
  - Fallback to current day if DaysOfWeek is null or empty

---

### 📦 2. DTOs
- `ScheduleDto`: Internal EF Core mapping.
- `ScheduleOutDto`: Public-facing version with all fields nullable.

---

### 🗃️ 3. EF Core & SQLite
- `SqliteContext` with:
  - DbSets for `ScheduleDto`, `AppointmentDto`, `EventDto`, `LocationDto`
- Relationships:
  - `ScheduleDto` ↔ `AppointmentDto` (1:N via `PendingAppointments` with `schedule_id` FK)
  - `AppointmentDto` ↔ `EventDto` (1:1)
  - `EventDto` ↔ `LocationDto` (1:1 optional)
  - Composite unique index on `LocationDto`

---

### ⏰ 4. Quartz.NET Cron Integration
- `ScheduleCronJobManager` handles scheduling:
  - Registers schedules and executes associated `Delegate`
  - Stores handlers with `ConcurrentDictionary`
  - Supports cancellation of jobs
- `ScheduleQuartzJob` executes registered task based on `ScheduleId`

---

### 🧪 5. Unit Tests
- Cover:
  - `Schedule` creation and validation
  - Structs (`HourMinute`, `DateRange`, etc.)
  - Quartz job scheduling via interface only
  - Multiple scheduled tasks with different parameters

---

## 📋 Planned Features

### ⚙️ 6. ScheduleService (WIP)
- Generates Appointments on demand.
- Example method: `GetAppointmentsBetween(DateOnly from, DateOnly to)`
- Respects `RepeatCount`, `LockedDates`, and `CancelledAppointments`

---

### 📆 7. Appointment Generation
- `AppointmentGeneratorService` (suggested name)
  - Generates recurring appointments
  - Does not persist to DB
  - Obeys recurrence logic

---

### 🛠️ 8. Schedule Rule Enforcement
- Combine `RepeatCount` and `EndDate` logic
- Apply `LockedDates`
- Support schedule overrides with new instances

---

### 🌐 9. CLI + MCP Server Integration
- Create commands like:
  - `schedule create`
  - `schedule list`
  - `appointment generate`

---

### 💬 10. Future Improvements (Post-MVP)
- `CUSTOM` recurrence support
- Notification/reminder system
- External calendar API integration
- Export features (JSON/CSV)
- MAUI UI app