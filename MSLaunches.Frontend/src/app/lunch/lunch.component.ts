import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';

import { LunchService } from './lunch.service';
import { Lunch, DailyTypedLunches, WeeklyLunches } from '@app/lunch/lunch.model';
import * as moment from 'moment';
import * as _ from 'lodash';

@Component({
  selector: 'app-home',
  templateUrl: './lunch.component.html',
  styleUrls: ['./lunch.component.scss']
})
export class LunchComponent implements OnInit {
  lunches: Array<WeeklyLunches>;
  isLoading: boolean;

  constructor(private lunchService: LunchService) { }

  ngOnInit() {
    this.isLoading = true;
    this.lunchService.getLaunches(new Date(2018, 1, 1), new Date(2018, 1, 0))
      .pipe(finalize(() => { this.isLoading = false; }))
      .subscribe(lunches => {
        this.lunches = this.mapToWeekly(lunches);
      });
  }

  private mapToWeekly(lunches: Array<Lunch>): Array<WeeklyLunches> {
    lunches = lunches.sort(this.typeSorter);

    const daily: Array<DailyTypedLunches> = _.map(
      // Groups by date
      _.groupBy(lunches, (result: Lunch) => result.date),
      // Maps to entity
      (value: Lunch[], key: string) => ({ date: new Date(key), lunches: value})
    );

    const weekly = _.map(
      // Groups by week
      _.groupBy(daily, (result: DailyTypedLunches) => moment(result.date, 'DD/MM/YYYY').startOf('isoWeek')),
      // maps to entity
      (value: DailyTypedLunches[], key: string) =>  ({ date: new Date(key), lunches: value})
    );

    weekly.forEach(dailyLunch => {
      let startOfTheweek: moment.Moment;
      if (!!dailyLunch.lunches && dailyLunch.lunches.length < 5 ) {
        startOfTheweek = this.firstDayofTheWeek(dailyLunch.date);
        for (let i = 0; i < 5; i++) {
          if (!dailyLunch.lunches.some(item =>
            item.date.getDate() === startOfTheweek.toDate().getDate())
          ) {
            dailyLunch.lunches.push({
              date: startOfTheweek.toDate(),
              lunches: new Array<Lunch>()
            });
          }
          startOfTheweek.add('days', 1);
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

  private firstDayofTheWeek(date: Date): moment.Moment {
    return moment(date, 'DD/MM/YYYY').startOf('isoWeek');
  }
}
