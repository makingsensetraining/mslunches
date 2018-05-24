import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { map, mapTo, mergeMap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs/observable/of';
import { tryStatement } from 'babel-types';
import { Lunch, DailyTypedLunches, WeeklyLunches } from '@app/lunch/lunch.model';
import * as moment from 'moment';
import * as _ from 'lodash';

@Injectable()
export class LunchService {
    constructor(private httpClient: HttpClient) {
    }

    getLaunches(startDate?: Date, endDate?: Date): Observable<Array<Lunch>> {
        return this.httpClient
            .get('/lunches')
            .pipe(map(this.mapToArrayOfLaunch.bind(this)));
    }

    getUserLunches(): Observable<Array<Lunch>> {
        const userId: string =
            JSON.parse(sessionStorage.getItem('credentials')).userId;

        return this.getLaunches()
            .pipe(
                mergeMap(menu => {
                return this.httpClient.get(`user/${userId}/lunches`).pipe(
                    map((value: any[]) =>
                        this.mergeUserLunchs(this.mapToArrayOfLaunch(value), menu))
                );
            })
        );
    }

    mapToWeekly(lunches: Array<Lunch>): Array<WeeklyLunches> {
        lunches = lunches.sort(this.typeSorter);
        const daily: Array<DailyTypedLunches> = _.map(
            // Groups by date
            _.groupBy(lunches, (result: Lunch) => result.date),
            // Maps to entity
            (value: Lunch[], key: string) => ({ date: new Date(key), lunches: value })
        );

        const weekly = _.map(
            // Groups by week
            _.groupBy(daily, (result: DailyTypedLunches) => moment(result.date, 'DD/MM/YYYY').startOf('isoWeek')),
            // maps to entity
            (value: DailyTypedLunches[], key: string) => ({ date: new Date(key), lunches: value })
        );

        return this.fillDates(weekly);
      }

      private mergeUserLunchs(userSelection: Array<Lunch>, menu: Array<Lunch>): Array<Lunch> {
        return new Array<Lunch>();
      }

      private fillDates(weekly: Array<WeeklyLunches>): Array<WeeklyLunches> {
        weekly.forEach(dailyLunch => {
            let startOfTheweek: moment.Moment;
            if (!!dailyLunch.lunches && dailyLunch.lunches.length < 5) {
                startOfTheweek = moment(dailyLunch.date, 'DD/MM/YYYY').startOf('isoWeek');
                for (let i = 0; i < 5; i++) {
                    if (!dailyLunch.lunches.some(item =>
                        item.date.getDate() === startOfTheweek.toDate().getDate())
                    ) {
                        dailyLunch.lunches.push({
                            date: startOfTheweek.toDate(),
                            lunches: new Array<Lunch>()
                        });
                    }
                    startOfTheweek.add(1, 'days');
                }
                dailyLunch.lunches = dailyLunch.lunches.sort(this.dateSorter);
            }
        });

        return weekly;
      }

    private typeSorter(a: Lunch, b: Lunch): number {
        if (a.type < b.type) {
            return -1;
        }
        if (b.type < a.type) {
            return 1;
        }
        return 0;
    }

    private dateSorter(a: DailyTypedLunches, b: DailyTypedLunches): number {
        if (a.date < b.date) {
            return -1;
        }
        if (b.date < a.date) {
            return 1;
        }
        return 0;
    }

    private mapToArrayOfLaunch(body: Array<any>): Array<Lunch> {
        let result: Array<Lunch> = new Array<Lunch>();

        result = body.map(this.mapToLaunch);

        return result;
    }

    private mapToLaunch(body: any): Lunch {
        let result: Lunch;
        result = {
            id: body.meal.id,
            description: body.meal.name,
            type: body.meal.mealType.description,
            date: body.date,
            isSelected: false,
            isSelectable: false
        };

        return result;
    }
}
