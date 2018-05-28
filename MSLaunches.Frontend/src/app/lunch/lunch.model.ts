export interface Lunch {
    id: String;
    userLunchId: String;
    description: String;
    type: String;
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
    id: String;
    lunchId: String;
}
