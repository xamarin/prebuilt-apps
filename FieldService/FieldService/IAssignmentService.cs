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
using System.Threading;
using System.Threading.Tasks;
using FieldService.Data;

namespace FieldService {
    /// <summary>
    /// Service for returning and saving assignment information
    /// </summary>
    public interface IAssignmentService {
        /// <summary>
        /// Gets a list of assignments
        /// </summary>
        Task<List<Assignment>> GetAssignmentsAsync (CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of items
        /// </summary>
        Task<List<Item>> GetItemsAsync (CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of items for an assignment
        /// </summary>
        Task<List<AssignmentItem>> GetItemsForAssignmentAsync (Assignment assignment, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of labor hours for an assignment
        /// </summary>
        Task<List<Labor>> GetLaborForAssignmentAsync (Assignment assignment, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of expenses for an assignment
        /// </summary>
        Task<List<Expense>> GetExpensesForAssignmentAsync (Assignment assignment, CancellationToken cancellationToken);

        /// <summary>
        /// Saves an assignment
        /// </summary>
        Task<int> SaveAssignmentAsync (Assignment assignment, CancellationToken cancellationToken);

        /// <summary>
        /// Saves a signature for an assignment
        /// </summary>
        Task<int> SaveSignatureAsync (Signature signature, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a signature for an assignment or null if none
        /// </summary>
        Task<Signature> GetSignatureAsync (Assignment assignment, CancellationToken cancellationToken);

        /// <summary>
        /// Saves an assignment item
        /// </summary>
        Task<int> SaveAssignmentItemAsync (AssignmentItem assignmentItem, CancellationToken cancellationToken);

        /// <summary>
        /// Saves a labor entry
        /// </summary>
        Task<int> SaveLaborAsync (Labor labor, CancellationToken cancellationToken);

        /// <summary>
        /// Saves an expense entry
        /// </summary>
        Task<int> SaveExpenseAsync (Expense expense, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes an assignment
        /// </summary>
        Task<int> DeleteAssignmentAsync (Assignment assignment, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes an assignment item
        /// </summary>
        Task<int> DeleteAssignmentItemAsync (AssignmentItem AssignmentItem, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a labor entry
        /// </summary>
        Task<int> DeleteLaborAsync (Labor labor, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes an expense entry
        /// </summary>
        Task<int> DeleteExpenseAsync (Expense expense, CancellationToken cancellationToken);

        /// <summary>
        /// Saves a timer entry to record an assignment
        /// </summary>
        Task<int> SaveTimerEntryAsync (TimerEntry entry, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a timer entry to stop recording an assignment
        /// </summary>
        Task<int> DeleteTimerEntryAsync (TimerEntry entry, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves the stored timer entry if there was one
        /// </summary>
        Task<TimerEntry> GetTimerEntryAsync (CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all the photos for an assignment
        /// </summary>
        Task<List<Photo>> GetPhotosForAssignmentAsync (Assignment assignment, CancellationToken cancellationToken);

        /// <summary>
        /// Saves a photo for an assignment
        /// </summary>
        Task<int> SavePhotoAsync (Photo photo, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a photo
        /// </summary>
        Task<int> DeletePhotoAsync (Photo photo, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the list of available documents
        /// </summary>
        Task<List<Document>> GetDocumentsAsync (CancellationToken cancellationToken);

        /// <summary>
        /// Gets the history for an assignment
        /// </summary>
        Task<List<AssignmentHistory>> GetAssignmentHistoryAsync (Assignment assignment, CancellationToken cancellationToken);
    }
}