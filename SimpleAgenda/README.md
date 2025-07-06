# 🗓️ SimpleAgenda – Project Overview

# Resumo Conceitual do Schedule

## O que é o Schedule?
- Entidade que define **padrões de recorrência** para agendamento de compromissos (Appointments).
- Não armazena compromissos diretamente, apenas regras para gerar esses compromissos sob demanda.

## O que o Schedule deve fazer?
- Definir o período de validade da recorrência (`StartDate` e `EndDate`).
- Controlar a frequência da recorrência (tipo e intervalo, ex: todo dia, toda semana, a cada 2 meses).
- Definir quais dias da semana geram eventos no caso de recorrência semanal.
- Determinar o horário da ocorrência no dia (via `TimeOfDay` / `HourMinute`).
- Permitir controle de exclusões (datas específicas que não geram evento mesmo que se enquadrem na regra).
- Permitir sobrescrever ocorrências específicas criando um novo schedule com `RepeatCount=1` e exclusão da data original.
- Trabalhar com limite de repetições (via `RepeatCount`) e/ou limite de data final.

## Prioridades definidas
- Ter estrutura clara e validada para as regras da recorrência (validação de datas, intervalos, horários).
- Implementar entidade Schedule que encapsule todas essas regras para facilitar o uso.
- Ter serviço para gerar Appointments baseado no Schedule (planejado para fase seguinte).
- Integrar Schedule com sistema de agendamento automático (Quartz.NET), permitindo passar uma ação (delegate) para execução no momento agendado.
- Garantir persistência coerente (DTO + banco) que reflita o modelo de domínio com mapeamentos corretos.

## Pontos para evoluir depois
- Serviço de geração sob demanda de Appointments com base no Schedule.
- Gestão avançada de exceções e overrides.
- Suporte a triggers mais complexos além do StartDate + TimeOfDay.
- Funcionalidades adicionais para o cron job (múltiplos tipos de trigger, regras complexas de repetição).

---
Esse é o guia para basear decisões técnicas e requisitos futuros.
---

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