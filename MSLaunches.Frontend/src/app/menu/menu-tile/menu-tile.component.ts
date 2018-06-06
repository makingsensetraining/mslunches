import { Component, Input, ViewChild, OnInit } from '@angular/core';
import { Lunch } from '@app/core/Models/lunch.model';
import { Meal } from '@app/core/Models/meal.model';
import { NgbTypeahead, NgbTypeaheadSelectItemEvent } from '@ng-bootstrap/ng-bootstrap';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';
import { debounceTime, distinctUntilChanged, merge, filter, map } from 'rxjs/operators';

@Component({
  selector: 'app-menu-tile',
  templateUrl: 'menu-tile.component.html',
  styleUrls: ['menu-tile.component.scss']
})
export class MenuTileComponent {
  @Input() meals: Array<Meal>;
  @Input() lunch: Lunch;

  @ViewChild('instance') instance: NgbTypeahead;
  focus$ = new Subject<string>();
  click$ = new Subject<string>();

  outputFormatter = (result: Meal) => result.name;
  inputFormatter = (value: Lunch) => value.mealName;

  search = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      merge(this.focus$),
      merge(this.click$.pipe(filter(() => !this.instance.isPopupOpen()))),
      map(term => (term === '' ? this.meals
        : this.meals.filter(v => v.name.toLowerCase().indexOf(term.toLowerCase()) > -1)).slice(0, 10))
    )

    selectItem = (event: NgbTypeaheadSelectItemEvent) => {
      this.lunch.mealId = event.item.id;
      this.lunch.mealName = event.item.name;
    }
}
