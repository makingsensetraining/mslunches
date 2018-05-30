import { Meal } from "@app/core/Models/meal.model";
import { MealType } from "@app/core/Models/meal-type.model";

export interface MealGrouped {
    mealType: MealType;
    meals: Array<Meal>;

}
