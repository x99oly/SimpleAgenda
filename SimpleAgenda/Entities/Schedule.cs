﻿/*
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
- **Funcionalidade descontinuada**
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
using System.Reflection.Metadata.Ecma335;

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
            int recurrenceInterval = 1, int recurrenceLitmit = int.MaxValue, IEnumerable<DayOfWeek>? daysOfWeek=null)
        {
            StartAndEndRangeDates = new(startDate, endDate ?? DateTime.UtcNow.AddYears(100));

            Recurrence = new Recurrence(
                recurrenceType:recurrenceType,
                recurrenceInterval:recurrenceInterval,
                recurrenceLimit:recurrenceLitmit,
                recurrenceTime:new HourMinute(hour, minutes),
                daysOfWeek:new DaysOfWeekCollection(daysOfWeek));
        }



        public IEnumerable<DayOfWeek> GetSchedulesDays() => Recurrence.DaysOfWeek.AsEnumerable();


    }

}

/*
# Checklist de Métodos para a Classe `Schedule`

## Ordem sugerida para desenvolvimento

- [ ] **1. `GetValidDateRange()`**  
  - Retorna o intervalo válido de datas da Schedule (`StartDate` até `EndDate`).

- [ok] **2. `GetValidDaysOfWeek()`**  
  - Retorna a lista dos dias da semana válidos para a recorrência (com fallback para `StartDate.DayOfWeek`).

- [ ] **3. `GenerateOccurrences(DateOnly from, DateOnly to)`**  
  - Gera as ocorrências (datas + horários) de Appointments dentro do intervalo fornecido, respeitando a regra da Schedule.

- [ ] **4. `IsOccurrence(DateTime date)`**  
  - Verifica se uma data específica está dentro da recorrência da Schedule.

- [ ] **5. `GetNextOccurrence(DateTime after)`**  
  - Retorna a próxima ocorrência válida após a data informada.

- [ ] **6. `GetTotalOccurrences()`**  
  - Calcula o total de ocorrências possíveis da Schedule, respeitando limites.

- [ ] **7. Métodos para exclusões (opcional)**  
  - Adicionar/remover datas excluídas (se implementado na classe).

- [ ] **8. `ToString()` / Representação legível**  
  - Retorna uma string descritiva da regra da Schedule para debugging ou exibição.

---

> **Observação:**  
> Métodos relacionados a exclusões, cancelamentos e sobrescritas são recomendados para serem implementados em camadas superiores, fora da classe `Schedule`.
 
*/