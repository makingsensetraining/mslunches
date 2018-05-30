import { UserLunch } from "@app/core/Models/user-lunch.model";

export interface DailyTypedLunches {
    date: Date;
    lunches: Array<UserLunch>;
}