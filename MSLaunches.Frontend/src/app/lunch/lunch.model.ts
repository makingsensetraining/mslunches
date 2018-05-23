export interface Lunch {
    description: String;
    type: String;
    date: Date;
    isSelected: Boolean;
}

export interface DailyTypedLunches {
    date: Date;
    lunches: Array<Lunch>;
}

export interface WeeklyLunches {
    lunches: Array<DailyTypedLunches>;
}
