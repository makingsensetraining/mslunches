import { Meal } from '@app/core/Models/meal.model';

export interface MealType {
  id: Number;
  name: string;
  meals: Array<Meal>;
}
