//
//  Copyright 2012  Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Data {
    /// <summary>
    /// Service for returning and saving assignment information
    /// </summary>
    public interface IAssignmentService {
        /// <summary>
        /// Gets a list of assignments
        /// </summary>
        Task<List<Assignment>> GetAssignmentsAsync ();

        /// <summary>
        /// Gets a list of items
        /// </summary>
        Task<List<Item>> GetItemsAsync ();

        /// <summary>
        /// Gets a list of items for an assignment
        /// </summary>
        Task<List<AssignmentItem>> GetItemsForAssignmentAsync (Assignment assignment);

        /// <summary>
        /// Gets a list of labor hours for an assignment
        /// </summary>
        Task<List<Labor>> GetLaborForAssignmentAsync (Assignment assignment);

        /// <summary>
        /// Gets a list of expenses for an assignment
        /// </summary>
        Task<List<Expense>> GetExpensesForAssignmentAsync (Assignment assignment);

        /// <summary>
        /// Saves an assignment
        /// </summary>
        Task<int> SaveAssignment (Assignment assignment);

        /// <summary>
        /// Saves an assignment item
        /// </summary>
        Task<int> SaveAssignmentItem (AssignmentItem assignmentItem);

        /// <summary>
        /// Saves a labor entry
        /// </summary>
        Task<int> SaveLabor (Labor labor);

        /// <summary>
        /// Saves an expense entry
        /// </summary>
        Task<int> SaveExpense (Expense expense);

        /// <summary>
        /// Deletes an assignment
        /// </summary>
        Task<int> DeleteAssignment (Assignment assignment);

        /// <summary>
        /// Deletes an assignment item
        /// </summary>
        Task<int> DeleteAssignmentItem (AssignmentItem AssignmentItem);

        /// <summary>
        /// Deletes a labor entry
        /// </summary>
        Task<int> DeleteLabor (Labor labor);

        /// <summary>
        /// Deletes an expense entry
        /// </summary>
        Task<int> DeleteExpense (Expense expense);

        /// <summary>
        /// Saves a timer entry to record an assignment
        /// </summary>
        Task<int> SaveTimerEntry (TimerEntry entry);

        /// <summary>
        /// Deletes a timer entry to stop recording an assignment
        /// </summary>
        Task<int> DeleteTimerEntry (TimerEntry entry);

        /// <summary>
        /// Retrieves the stored timer entry if there was one
        /// </summary>
        Task<TimerEntry> GetTimerEntryAsync ();

        /// <summary>
        /// Retrieves all the photos for an assignment
        /// </summary>
        Task<List<Photo>> GetPhotosForAssignmentAsync (Assignment assignment);

        /// <summary>
        /// Saves a photo for an assignment
        /// </summary>
        Task<int> SavePhoto (Photo photo);

        /// <summary>
        /// Deletes a photo
        /// </summary>
        Task<int> DeletePhoto (Photo photo);

        /// <summary>
        /// Gets the list of available documents
        /// </summary>
        Task<List<Document>> GetDocumentsAsync ();

        /// <summary>
        /// Gets the history for an assignment
        /// </summary>
        Task<List<AssignmentHistory>> GetAssignmentHistoryAsync (Assignment assignment);
    }
}