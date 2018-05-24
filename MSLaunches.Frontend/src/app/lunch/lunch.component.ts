import { Component, OnInit } from '@angular/core';
import { filter, finalize } from 'rxjs/operators';

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
        this.lunches = this.lunchService.mapToWeekly(lunches);
      });
  }

  setLunchSelected(lunch: Lunch) {
    let weeklyLunches: WeeklyLunches;
    let lunchesByDay: DailyTypedLunches;
    const lunchDate: Date = new Date(lunch.date);
    const startOfTheWeek: Date = this.firstDayOfTheWeek(lunch.date, 'YYYY-MM-DD').toDate();

    weeklyLunches = this.lunches.find(l =>
      l.date.getDate() === startOfTheWeek.getDate()
    );

    lunchesByDay = weeklyLunches.lunches.find(
      l => l.date.getDate() === lunchDate.getDate()
    );

    lunchesByDay.lunches.forEach(l => {
      if (l.id !== lunch.id) {
        l.isSelected = false;
      }
    });
  }

  private firstDayOfTheWeek(date: Date, format?: moment.MomentFormatSpecification): moment.Moment {
    if (!format) {
      format = 'DD/MM/YYYY';
    }
    return moment(date, format).startOf('isoWeek');
  }
}
