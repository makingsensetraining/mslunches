import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';

import { Lunch } from '@app/core/Models/lunch.model';
import { WeeklyLunches } from '@app/core/Models/weekly-lunches.model';

export class MockLunchService {
    private mockedLunches: Array<Lunch> = this.getMockedLunches(this.defaultDate());
    private mockedWeeklyLunches: Array<WeeklyLunches> = [{
        date: this.defaultDate(),
        lunches: [
            {
                date: this.defaultDate(),
                lunches: this.getMockedLunches(this.defaultDate())
            },
            {
                date: this.defaultDate(6),
                lunches: this.getMockedLunches(this.defaultDate(6))
            },
            {
                date: this.defaultDate(7),
                lunches: this.getMockedLunches(this.defaultDate(7))
            }
        ]
    }];

    getLunches(startDate?: Date, endDate?: Date): Observable<Array<Lunch>> {
        return of(this.mockedLunches);
    }

    getUserLunches(): Observable<Array<Lunch>> {
        return of(this.mockedLunches);
    }

    mapToWeekly(lunches: Array<Lunch>): Array<WeeklyLunches> {
        return this.mockedWeeklyLunches;
    }

    save(lunch: Lunch): Observable<string> {
        return of('someString');
    }

    delete(lunch: Lunch): Observable<string> {
        return of('someString');
    }

    private getMockedLunches(date?: Date) {
        return [
            {
                date: !!date ? date : this.defaultDate(),
                description: 'desc1',
                id: 'id1',
                isSelectable: true,
                isSelected: true,
                type: 'someType',
                userLunchId: 'userid',
                mealId: 'someMealId'
            },
            {
                date: new Date(Date.now()),
                description: 'desc2',
                id: 'id2',
                isSelectable: false,
                isSelected: false,
                type: 'someType1',
                userLunchId: 'userid',
                mealId: 'someMealId'
            },
        ];
    }

    private defaultDate(day?: Number): Date {
        if (!day) {
            day = 5;
        }
        let stringyDay: String;
        if (day < 10) {
            stringyDay = '0' + day;
        } else {
            stringyDay = day.toString();
        }
        return new Date('2018-06-' + stringyDay);
    }
}
