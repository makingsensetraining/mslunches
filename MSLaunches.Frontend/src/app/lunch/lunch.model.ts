export interface Lunch {
    id: string;
    userLunchId: string;
    description: string;
    type: string;
    date: Date;
    isSelected: Boolean;
    isSelectable: Boolean;
}

export interface DailyTypedLunches {
    date: Date;
    lunches: Array<Lunch>;
}

export interface WeeklyLunches {
    date: Date;
    lunches: Array<DailyTypedLunches>;
}

export interface UserSelection {
    id: string;
    lunchId: string;
}
