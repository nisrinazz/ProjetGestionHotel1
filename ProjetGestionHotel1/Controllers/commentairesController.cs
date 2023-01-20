using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using ProjetGestionHotel1.Models;

namespace ProjetGestionHotel1.Controllers
{
    public class commentairesController : Controller
    {

        public partial class SentimentModel
        {
            /// <summary>
            /// model input class for SentimentModel.
            /// </summary>
            #region model input class
            public class ModelInput
            {
                [LoadColumn(0)]
                [ColumnName(@"score")]
                public float Score { get; set; }

                [LoadColumn(1)]
                [ColumnName(@"new_reviews")]
                [Display(Name = "Message")]
                [DataType(DataType.MultilineText)]
                [Required]
                public string New_reviews { get; set; }

            }

            #endregion

            /// <summary>
            /// model output class for SentimentModel.
            /// </summary>
            #region model output class
            public class ModelOutput
            {
                [ColumnName(@"score")]
                public uint Score { get; set; }


                [ColumnName(@"PredictedLabel")]
                public float Prediction { get; set; }


            }

            #endregion

            private static string MLNetModelPath = Path.GetFullPath("C:\\Users\\azzai\\source\\repos\\ProjetGestionHotel1\\ProjetGestionHotel1\\SentimentModel.zip");

            public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);

            /// <summary>
            /// Use this method to predict on <see cref="ModelInput"/>.
            /// </summary>
            /// <param name="input">model input.</param>
            /// <returns><seealso cref=" ModelOutput"/></returns>
            public static ModelOutput Predict(ModelInput input)
            {
                var predEngine = PredictEngine.Value;
                return predEngine.Predict(input);
            }

            private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
            {
                var mlContext = new MLContext();
                ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var _);
                return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
            }
            /// <summary>
            /// Retrains model using the pipeline generated as part of the training process. For more information on how to load data, see aka.ms/loaddata.
            /// </summary>
            /// <param name="mlContext"></param>
            /// <param name="trainData"></param>
            /// <returns></returns>
            public static ITransformer RetrainPipeline(MLContext mlContext, IDataView trainData)
            {
                var pipeline = BuildPipeline(mlContext);
                var model = pipeline.Fit(trainData);

                return model;
            }

            /// <summary>
            /// build the pipeline that is used from model builder. Use this function to retrain model.
            /// </summary>
            /// <param name="mlContext"></param>
            /// <returns></returns>
            public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
            {
                // Data process configuration with pipeline data transformations
                var pipeline = mlContext.Transforms.Text.FeaturizeText(inputColumnName: @"new_reviews", outputColumnName: @"new_reviews")
                                        .Append(mlContext.Transforms.Concatenate(@"Features", new[] { @"new_reviews" }))
                                        .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: @"score", inputColumnName: @"score"))
                                        .Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(new LbfgsMaximumEntropyMulticlassTrainer.Options() { L1Regularization = 0.03125F, L2Regularization = 0.03125F, LabelColumnName = @"score", FeatureColumnName = @"Features" }))
                                        .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: @"PredictedLabel", inputColumnName: @"PredictedLabel"));

                return pipeline;
            }

        }


        private gestion_hotelEntities db = new gestion_hotelEntities();

        // GET: commentaires
        public ActionResult Index()
        {
            var commentaire = db.commentaire.Include(c => c.client);
            return View(Enumerable.Reverse(commentaire.ToList()).ToList());
        }

        // GET: commentaires/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            commentaire commentaire = db.commentaire.Find(id);
            if (commentaire == null)
            {
                return HttpNotFound();
            }
            return View(commentaire);
        }

        // GET: commentaires/Create
        [ChildActionOnly]
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: commentaires/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public float prediction(SentimentModel.ModelInput input)
        {
           
            return  SentimentModel.Predict(input).Prediction;
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SentimentModel.ModelInput input)
        {
            if (Session["user"] != null)
            {
                if (ModelState.IsValid)
                {
                    commentaire comment = new commentaire();
                    client c = (client)Session["user"];
                    comment.id_client = c.id_client;
                    comment.text_commentaire = input.New_reviews;
                    comment.score = (int)prediction(input);
                    if (comment.score == 1)
                    { comment.prediction = "positive"; }
                    else if (comment.score == 0)
                    { comment.prediction = "neutral"; }
                    else
                    {
                        comment.prediction = "negative";
                    }
                    db.commentaire.Add(comment);
                    db.SaveChanges();

                }
                return View(input);
            }
            else
            {
                return RedirectToAction("Login","Account");
            }
            }
            
        

        // GET: commentaires/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            commentaire commentaire = db.commentaire.Find(id);
            if (commentaire == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_client = new SelectList(db.client, "id_client", "nom_client", commentaire.id_client);
            return View(commentaire);
        }

        // POST: commentaires/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_commentaire,id_client,text_commentaire,score,prediction")] commentaire commentaire)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commentaire).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_client = new SelectList(db.client, "id_client", "nom_client", commentaire.id_client);
            return View(commentaire);
        }

        // GET: commentaires/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            commentaire commentaire = db.commentaire.Find(id);
            if (commentaire == null)
            {
                return HttpNotFound();
            }
            db.commentaire.Remove(commentaire);
            db.SaveChanges();
            return RedirectToAction("Index","Home");

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
