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

    let daily: Array<DailyTypedLunches> = _.map(
      // Groups by date
      _.groupBy(lunches, (result: Lunch) => result.date),
      (value: Lunch[], key: string) => ({ date: new Date(key), lunches: value})
    );

    daily = daily.sort(this.dateSorter);

    return _.map(
      _.groupBy(daily, (result: DailyTypedLunches) => moment(result.date, 'DD/MM/YYYY').startOf('isoWeek')),
      (value: DailyTypedLunches[], key: string) =>  ({ date: new Date(key), lunches: value})
    );
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
}
