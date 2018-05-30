import { DailyTypedLunches } from './daily-typed-lunches.model';

export interface WeeklyLunches {
    date: Date;
    lunches: Array<DailyTypedLunches>;
}