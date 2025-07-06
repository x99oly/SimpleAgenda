/*
 # ✅ Regras de Negócio – Entidade Schedule (SimpleAgenda)
---
## 🧱 Estrutura Geral
- Schedule é a **entidade responsável por definir padrões de recorrência** de `Appointment`s.
- Os `Appointments` não são salvos por padrão; eles são **gerados sob demanda** com base nas regras do Schedule.
---
## 🕒 1. Datas e Frequência
### 1.1 `StartDate`
- Define a **data de início da recorrência**.
- Não pode ser anterior ao momento atual (`DateTime.UtcNow`).
### 1.2 `EndDate`
- Opcional.
- Se fornecida, **deve ser maior que `StartDate`**.
- Se omitida, assume um valor padrão futuro (ex: +100 anos).
### 1.3 `RecurrenceInterval`
- Inteiro ≥ 1.
- Define o **espaço entre repetições** conforme o `RecurrenceType`.
Exemplos:
- RecurrenceType = Daily, Interval = 1 → todo dia
- RecurrenceType = Weekly, Interval = 2 → a cada 2 semanas
---
## 🔁 2. Tipo de Recorrência
### 2.1 `RecurrenceType`
Valores possíveis:
- `DAILY`
- `WEEKLY`
- `MONTHLY`
- `YEARLY`
- `CUSTOM` (reservado para casos futuros)
### 2.2 `DaysOfWeek`
- Usado **APENAS SE** `RecurrenceType == WEEKLY`.
- Lista de dias da semana em que ocorre (ex: `[Wednesday, Friday]`).
- Se não for fornecida, assume `StartDate.DayOfWeek`.
---
## ⏰ 3. Horário da Ocorrência
- O campo `TimeOfDay` define o **horário do dia** em que a recorrência deve ocorrer (ex: 08:00).
- `Appointment.DateTime` final = `Date + TimeOfDay`.
---
## 🔁 4. Número de Repetições
### 4.1 `RepeatCount`
- Opcional.
- Define um **limite de ocorrências** da série.
- Não pode ser usado junto com `EndDate` de forma conflitante (o menor dos dois será respeitado).
---
## 🔒 5. Controle de Exceções
### 5.1 `Exclusions`
- Lista de `DateOnly`.
- Se uma data estiver nessa lista, **nenhum Appointment será gerado nela**, mesmo que caia na regra.
### 5.2 Overrides com Novo Schedule
- Se um `Appointment` for alterado (ex: horário ou conteúdo diferente), a estratégia é:
  1. Criar um novo `Schedule` com `RepeatCount = 1` e `StartDate = data modificada`
  2. Adicionar essa data à lista de `Exclusions` do `Schedule` original
---
## 🔄 6. Geração Sob Demanda
- `ScheduleService` (ou equivalente) deve expor métodos como:
```csharp
IEnumerable<Appointment> GetAppointmentsBetween(DateOnly from, DateOnly to)
 */
using SimpleAgenda.Enums;
using SimpleAgenda.Exceptions;

namespace SimpleAgenda.Entities
{
    internal class Schedule
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateRange StartAndEndRangeDates {  get; private set; }
        public Recurrence Recurrence { get; private set; }
        public List<Appointment> CancelledAppointments { get; private set; } = [];
        public List<Appointment> PendingAppointments { get; private set; } = [];

        /// <summary>
        /// Inicializa uma coleção de dias da semana a partir de uma lista.
        /// 
        /// <para>
        /// Se a lista <paramref name="days"/> for nula ou vazia, o construtor aplica um fallback silencioso
        /// usando o dia atual (UTC) como único dia válido. Essa abordagem evita erros e facilita o uso
        /// padrão sem exigir que o consumidor sempre especifique os dias.
        /// </para>
        /// </summary>
        /// <param name="days">Coleção de dias da semana a serem incluídos na recorrência. Pode ser nula ou vazia.</param>
        public Schedule(DateTime startDate, int hour, int minutes, DateTime? endDate = null, 
            RecurrenceTypeEnum recurrenceType = RecurrenceTypeEnum.WEEKLY, 
            int recurrenceInterval = 1, IEnumerable<DayOfWeek>? daysOfWeek=null)
        {
            StartAndEndRangeDates = new(startDate, endDate ?? DateTime.UtcNow.AddYears(100));

            Recurrence = new Recurrence(
                recurrenceType,
                recurrenceInterval,
                new HourMinute(hour, minutes),
                new DaysOfWeekCollection(daysOfWeek));
        }


    }

    internal readonly struct Recurrence(RecurrenceTypeEnum recurrenceType, int recurrenceInterval, HourMinute recurrenceTime, 
        DaysOfWeekCollection daysOfWeek)
    {
        public readonly RecurrenceTypeEnum RecurrenceType { get; init; } = recurrenceType;
        public readonly int RecurrenceInterval { get; init; } = RecurrenceIntervalValidador(recurrenceInterval);
        public readonly HourMinute RecurrenceTime { get; init; } = recurrenceTime;
        public readonly DaysOfWeekCollection DaysOfWeek { get; init; } = daysOfWeek;

        private static int RecurrenceIntervalValidador(int recurrenceInterval)
        {
            if (recurrenceInterval <= 0)
                throw new RecurrenceException(recurrence:recurrenceInterval);

            return recurrenceInterval;
        }

    }

    internal readonly record struct DateRange(DateTime StartDate, DateTime EndDate)
    {
        public readonly DateTime StartDate = StartDate >= DateTime.UtcNow
            ? StartDate 
            : throw new DateRangeException(
                $"The provided date '{StartDate}' cannot be early than the current date '{DateTime.UtcNow}-UTC'.");

        public readonly DateTime EndDate =  EndDate > StartDate
            ? EndDate
            : throw new DateRangeException(
                $"The provided 'End Date' is null or smaller than 'Start Date'.");
    }

    internal record struct HourMinute(int Hours, int Minutes)
    {
        public readonly int Hour = Hours is < 24 and >= 0
            ? Hours
            : throw new InvalidHourException(hour:Hours);
        public readonly int Minute = Minutes is < 60 and >= 0
            ? Minutes
            : throw new InvalidMinuteException(minute: Hours);

        public readonly TimeSpan AsTimeSpan()
            => new(Hour, Minute, 0);

        public static HourMinute FromTimeSpan(TimeSpan ts)
            => new((int)ts.TotalHours, ts.Minutes);

        public readonly override string ToString() => $"{Hour:D2}:{Minute:D2}";
    }

    internal readonly struct DaysOfWeekCollection
    {
        private readonly HashSet<DayOfWeek> _days;

        public DaysOfWeekCollection(IEnumerable<DayOfWeek>? days)
        {
            if (days is null || !days.Any())
                _days = [DateTime.UtcNow.DayOfWeek];
            else
                _days = [..days];
        }

        public static DaysOfWeekCollection FromDate(DateTime date)
            => new([date.DayOfWeek]);

        public bool Contains(DayOfWeek day) => _days.Contains(day);

        public IEnumerable<DayOfWeek> AsEnumerable() => _days.OrderBy(d => d);

        public override string ToString()
            => string.Join(", ", AsEnumerable());
    }

}
