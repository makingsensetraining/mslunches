import { Component, Input, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';

import { MomentFormatSpecification, Moment } from 'moment';
const moment = require('moment');

import { DailyTypedLunches } from '@app/core/Models/daily-typed-lunches.model';
import { UserLunch } from '@app/core/Models/user-lunch.model';
import { WeeklyLunches } from '@app/core/Models/weekly-lunches.model';
import { LunchService } from '@app/lunch/lunch.service';

@Component({
  selector: 'app-lunch',
  templateUrl: './lunch.component.html',
  styleUrls: ['./lunch.component.scss']
})
export class LunchComponent implements OnInit {
  @Input() canBeSelected: boolean;

  lunches: Array<WeeklyLunches>;
  isLoading: boolean;

  constructor(private lunchService: LunchService) {
  }

  ngOnInit() {
    this.isLoading = true;
    this.lunchService.getUserLunches()
      .pipe(finalize(() => { this.isLoading = false; }))
      .subscribe(lunches => {
        this.lunches = this.lunchService.mapToWeekly(lunches);
      });

    if (this.canBeSelected === undefined) {
      this.canBeSelected = true;
    }
  }

  resolveWeekLabel(date: Date): string {
    let timeDiff = date.getTime() - moment(Date.now()).startOf('isoWeek').toDate().getTime();
    if (timeDiff === 0) {
      return 'This week';
    } else if (timeDiff < 0) {
      timeDiff = -timeDiff;
      const days = Math.round(new Date(timeDiff).getDate() / 7);
      if (days === 1) {
        return 'Last week';
      }

      return `Last ${days} weeks`;
    } else {
      const days = Math.round(new Date(timeDiff).getDate() / 7);
      if (days === 1) {
        return 'Next week';
      }

      return `Next ${days} weeks`;
    }
  }

  setLunchSelected(lunch: UserLunch) {
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

    this.isLoading = true;
    if (lunch.isSelected) {
      this.lunchService.save(lunch).subscribe(
        a => {
          lunch.userLunchId = a;
          this.isLoading = false;
        },
        (err: any) => lunch.isSelected = false);
    } else {
      this.lunchService.delete(lunch).subscribe(
        () => {
          lunch.userLunchId = null;
        },
        a => lunch.isSelected = true,
        () => this.isLoading = false
      );
    }
  }

  private firstDayOfTheWeek(date: Date, format?: MomentFormatSpecification): Moment {
    if (!format) {
      format = 'DD/MM/YYYY';
    }
    return moment(date, format).startOf('isoWeek');
  }
}
